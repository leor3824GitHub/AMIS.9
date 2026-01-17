using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Domain;

public sealed class AnnualProcurementPlanHeader : AuditableEntity, IAggregateRoot
{
    public string ControlNumber { get; private set; } = string.Empty;
    public int FiscalYear { get; private set; }

    public AnnualProcurementPlanStatus Status { get; private set; } = AnnualProcurementPlanStatus.Draft;
    public BudgetType BudgetType { get; private set; }

    public decimal TotalBudget { get; private set; }

    public Guid PreparedByUserId { get; private set; }
    public DateTimeOffset? SubmissionDate { get; private set; }
    public Guid? ApprovedByUserId { get; private set; }
    public DateTimeOffset? ApprovalDate { get; private set; }
    public string? RejectionReason { get; private set; }

    private readonly List<AnnualProcurementPlanItem> _items = new();
    public IReadOnlyCollection<AnnualProcurementPlanItem> Items => _items.AsReadOnly();

    private AnnualProcurementPlanHeader() { }

    private AnnualProcurementPlanHeader(
        Guid id,
        string controlNumber,
        int fiscalYear,
        BudgetType budgetType,
        Guid preparedByUserId)
    {
        Id = id;
        SetHeaderFields(controlNumber, fiscalYear, budgetType);
        PreparedByUserId = preparedByUserId;
        Status = AnnualProcurementPlanStatus.Draft;
        RecomputeTotals();
    }

    public static AnnualProcurementPlanHeader Create(
        string controlNumber,
        int fiscalYear,
        BudgetType budgetType,
        Guid preparedByUserId)
        => new(Guid.NewGuid(), controlNumber, fiscalYear, budgetType, preparedByUserId);

    public AnnualProcurementPlanHeader UpdateHeader(int fiscalYear, BudgetType budgetType)
    {
        EnsureEditable();
        SetHeaderFields(ControlNumber, fiscalYear, budgetType);
        return this;
    }

    public AnnualProcurementPlanItem AddItem(
        Guid departmentId,
        string departmentName,
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

        var item = new AnnualProcurementPlanItem(Id, departmentId, departmentName, papCode, description, projectType, quantity, unitOfMeasure, unitCost, mode, isEarlyProcurement, scheduleMonth, fundingSource, remarks);
        _items.Add(item);
        RecomputeTotals();
        return item;
    }

    public void UpdateItem(
        Guid itemId,
        Guid departmentId,
        string departmentName,
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
        if (item is null) throw new KeyNotFoundException("APP item not found.");

        item.Update(departmentId, departmentName, papCode, description, projectType, quantity, unitOfMeasure, unitCost, mode, isEarlyProcurement, scheduleMonth, fundingSource, remarks);
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
        if (Status != AnnualProcurementPlanStatus.Draft)
            throw new InvalidOperationException("Only draft annual procurement plans can be submitted.");

        Status = AnnualProcurementPlanStatus.PendingApproval;
        SubmissionDate = DateTimeOffset.UtcNow;
    }

    public void Approve(Guid approvedByUserId)
    {
        if (Status != AnnualProcurementPlanStatus.PendingApproval)
            throw new InvalidOperationException("Only submitted annual procurement plans can be approved.");

        Status = AnnualProcurementPlanStatus.Approved;
        ApprovedByUserId = approvedByUserId;
        ApprovalDate = DateTimeOffset.UtcNow;
    }

    public void Reject(Guid rejectedByUserId, string reason)
    {
        if (Status != AnnualProcurementPlanStatus.PendingApproval)
            throw new InvalidOperationException("Only submitted annual procurement plans can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason is required.", nameof(reason));

        Status = AnnualProcurementPlanStatus.Rejected;
        ApprovedByUserId = rejectedByUserId;
        ApprovalDate = DateTimeOffset.UtcNow;
        RejectionReason = reason;
    }

    public void Cancel()
    {
        if (Status == AnnualProcurementPlanStatus.Approved)
            throw new InvalidOperationException("Approved annual procurement plans cannot be cancelled.");
        if (Status == AnnualProcurementPlanStatus.Cancelled)
            return; // Already cancelled

        Status = AnnualProcurementPlanStatus.Cancelled;
    }

    public void RevertToDraft()
    {
        if (Status != AnnualProcurementPlanStatus.Rejected)
            throw new InvalidOperationException("Only rejected annual procurement plans can be reverted to draft.");

        Status = AnnualProcurementPlanStatus.Draft;
        SubmissionDate = null;
        ApprovedByUserId = null;
        ApprovalDate = null;
        RejectionReason = null;
    }

    private void EnsureEditable()
    {
        if (Status != AnnualProcurementPlanStatus.Draft)
            throw new InvalidOperationException("Only draft annual procurement plans can be modified.");
    }

    private void SetHeaderFields(string controlNumber, int fiscalYear, BudgetType budgetType)
    {
        if (string.IsNullOrWhiteSpace(controlNumber)) throw new ArgumentException("Control number is required.", nameof(controlNumber));
        if (fiscalYear <= 0) throw new ArgumentOutOfRangeException(nameof(fiscalYear), "Fiscal year is required.");

        ControlNumber = controlNumber;
        FiscalYear = fiscalYear;
        BudgetType = budgetType;
    }

    private void RecomputeTotals()
    {
        TotalBudget = _items.Sum(x => x.EstimatedBudget);
    }
}

