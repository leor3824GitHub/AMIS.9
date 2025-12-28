using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Catalog.Domain;

public sealed class AnnualProcurementPlanItem : AuditableEntity
{
    public Guid PlanHeaderId { get; private set; }

    public Guid DepartmentId { get; private set; }
    public string DepartmentName { get; private set; } = string.Empty;

    public string? PapCode { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public ProjectType ProjectType { get; private set; }

    public int Quantity { get; private set; }
    public string UnitOfMeasure { get; private set; } = string.Empty;
    public decimal UnitCost { get; private set; }
    public decimal EstimatedBudget { get; private set; }

    public string Mode { get; private set; } = string.Empty;
    public bool IsEarlyProcurement { get; private set; }
    public string ScheduleMonth { get; private set; } = string.Empty;
    public string FundingSource { get; private set; } = string.Empty;
    public string? Remarks { get; private set; }

    private AnnualProcurementPlanItem() { }

    internal AnnualProcurementPlanItem(
        Guid planHeaderId,
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
        Id = Guid.NewGuid();
        PlanHeaderId = planHeaderId;
        Update(departmentId, departmentName, papCode, description, projectType, quantity, unitOfMeasure, unitCost, mode, isEarlyProcurement, scheduleMonth, fundingSource, remarks);
    }

    internal void Update(
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
        if (departmentId == Guid.Empty) throw new ArgumentException("DepartmentId is required.", nameof(departmentId));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Description is required.", nameof(description));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        if (string.IsNullOrWhiteSpace(unitOfMeasure)) throw new ArgumentException("UnitOfMeasure is required.", nameof(unitOfMeasure));
        if (unitCost <= 0) throw new ArgumentOutOfRangeException(nameof(unitCost), "UnitCost must be greater than zero.");
        if (string.IsNullOrWhiteSpace(mode)) throw new ArgumentException("Mode is required.", nameof(mode));
        if (string.IsNullOrWhiteSpace(scheduleMonth)) throw new ArgumentException("ScheduleMonth is required.", nameof(scheduleMonth));
        if (string.IsNullOrWhiteSpace(fundingSource)) throw new ArgumentException("FundingSource is required.", nameof(fundingSource));

        DepartmentId = departmentId;
        DepartmentName = departmentName ?? string.Empty;
        PapCode = string.IsNullOrWhiteSpace(papCode) ? null : papCode;
        Description = description;
        ProjectType = projectType;
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
        UnitCost = unitCost;
        Mode = mode;
        IsEarlyProcurement = isEarlyProcurement;
        ScheduleMonth = scheduleMonth;
        FundingSource = fundingSource;
        Remarks = string.IsNullOrWhiteSpace(remarks) ? null : remarks;

        EstimatedBudget = Quantity * UnitCost;
    }
}
