using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public sealed class ProcurementPlanHeader : AuditableEntity, IAggregateRoot
{
    public string ControlNumber { get; private set; } = string.Empty;
    public int FiscalYear { get; private set; }
    public Guid DepartmentId { get; private set; }
    public string DepartmentName { get; private set; } = string.Empty;

    public ProcurementPlanStatus Status { get; private set; } = ProcurementPlanStatus.Draft;
    public bool IsSupplemental { get; private set; }
    public BudgetType BudgetType { get; private set; }

    public decimal TotalBudget { get; private set; }

    public Guid PreparedByUserId { get; private set; }
    public DateTimeOffset? SubmissionDate { get; private set; }
    public Guid? ApprovedByUserId { get; private set; }
    public DateTimeOffset? ApprovalDate { get; private set; }
    public string? RejectionReason { get; private set; }

    private readonly List<ProcurementPlanItem> _items = new();
    public IReadOnlyCollection<ProcurementPlanItem> Items => _items.AsReadOnly();

    private ProcurementPlanHeader() { }

    private ProcurementPlanHeader(
        Guid id,
        string controlNumber,
        int fiscalYear,
        Guid departmentId,
        string departmentName,
        bool isSupplemental,
        BudgetType budgetType,
        Guid preparedByUserId)
    {
        Id = id;
        SetHeaderFields(controlNumber, fiscalYear, departmentId, departmentName, isSupplemental, budgetType);
        PreparedByUserId = preparedByUserId;
        Status = ProcurementPlanStatus.Draft;
        RecomputeTotals();
    }

    public static ProcurementPlanHeader Create(
        string controlNumber,
        int fiscalYear,
        Guid departmentId,
        string departmentName,
        bool isSupplemental,
        BudgetType budgetType,
        Guid preparedByUserId)
        => new(Guid.NewGuid(), controlNumber, fiscalYear, departmentId, departmentName, isSupplemental, budgetType, preparedByUserId);

    public ProcurementPlanHeader UpdateHeader(
        Guid departmentId,
        string departmentName,
        bool isSupplemental,
        BudgetType budgetType)
    {
        EnsureEditable();
        SetHeaderFields(ControlNumber, FiscalYear, departmentId, departmentName, isSupplemental, budgetType);
        return this;
    }

    public ProcurementPlanItem AddItem(
        string? papCode,
        string description,
        ProjectType projectType,
        int quantity,
        string unitOfMeasure,
        decimal unitCost,
        string mode,
        bool isEarlyProcurement,
        string scheduleMonth,
        string fundingSource,
        string? remarks)
    {
        EnsureEditable();

        var item = new ProcurementPlanItem(Id, papCode, description, projectType, quantity, unitOfMeasure, unitCost, mode, isEarlyProcurement, scheduleMonth, fundingSource, remarks);
        _items.Add(item);
        RecomputeTotals();
        return item;
    }

    public void UpdateItem(
        Guid itemId,
        string? papCode,
        string description,
        ProjectType projectType,
        int quantity,
        string unitOfMeasure,
        decimal unitCost,
        string mode,
        bool isEarlyProcurement,
        string scheduleMonth,
        string fundingSource,
        string? remarks)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null) throw new KeyNotFoundException("PPMP item not found.");

        item.Update(papCode, description, projectType, quantity, unitOfMeasure, unitCost, mode, isEarlyProcurement, scheduleMonth, fundingSource, remarks);
        RecomputeTotals();
    }

    public void DeleteItem(Guid itemId)
    {
        EnsureEditable();

        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item is null) return;

        _items.Remove(item);
        RecomputeTotals();
    }

    public void Submit()
    {
        if (Status != ProcurementPlanStatus.Draft)
            throw new InvalidOperationException("Only draft plans can be submitted.");

        Status = ProcurementPlanStatus.PendingApproval;
        SubmissionDate = DateTimeOffset.UtcNow;
    }

    public void Approve(Guid approvedByUserId)
    {
        if (Status != ProcurementPlanStatus.PendingApproval)
            throw new InvalidOperationException("Only submitted plans can be approved.");

        Status = ProcurementPlanStatus.Approved;
        ApprovedByUserId = approvedByUserId;
        ApprovalDate = DateTimeOffset.UtcNow;
    }

    public void Reject(Guid rejectedByUserId, string reason)
    {
        if (Status != ProcurementPlanStatus.PendingApproval)
            throw new InvalidOperationException("Only submitted plans can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required.", nameof(reason));

        Status = ProcurementPlanStatus.Rejected;
        ApprovedByUserId = rejectedByUserId;
        ApprovalDate = DateTimeOffset.UtcNow;
        RejectionReason = reason;
    }

    public void Cancel()
    {
        if (Status == ProcurementPlanStatus.Approved)
            throw new InvalidOperationException("Approved plans cannot be cancelled.");
        if (Status == ProcurementPlanStatus.Cancelled)
            return; // Already cancelled

        Status = ProcurementPlanStatus.Cancelled;
    }

    public void RevertToDraft()
    {
        if (Status != ProcurementPlanStatus.Rejected)
            throw new InvalidOperationException("Only rejected plans can be reverted to draft.");

        Status = ProcurementPlanStatus.Draft;
        SubmissionDate = null;
        ApprovedByUserId = null;
        ApprovalDate = null;
        RejectionReason = null;
    }

    private void EnsureEditable()
    {
        if (Status != ProcurementPlanStatus.Draft)
            throw new InvalidOperationException("Only draft procurement plans can be modified.");
    }

    private void SetHeaderFields(
        string controlNumber,
        int fiscalYear,
        Guid departmentId,
        string departmentName,
        bool isSupplemental,
        BudgetType budgetType)
    {
        if (string.IsNullOrWhiteSpace(controlNumber)) throw new ArgumentException("Control number is required.", nameof(controlNumber));
        if (fiscalYear <= 0) throw new ArgumentOutOfRangeException(nameof(fiscalYear), "Fiscal year is required.");
        if (departmentId == Guid.Empty) throw new ArgumentException("DepartmentId is required.", nameof(departmentId));

        ControlNumber = controlNumber;
        FiscalYear = fiscalYear;
        DepartmentId = departmentId;
        DepartmentName = departmentName ?? string.Empty;
        IsSupplemental = isSupplemental;
        BudgetType = budgetType;
    }

    private void RecomputeTotals()
    {
        TotalBudget = _items.Sum(x => x.EstimatedBudget);
    }
}
