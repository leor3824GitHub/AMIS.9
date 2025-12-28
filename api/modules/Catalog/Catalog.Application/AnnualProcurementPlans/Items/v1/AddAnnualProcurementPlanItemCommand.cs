using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Items.v1;

public sealed class AddAnnualProcurementPlanItemCommand : IRequest<AddAnnualProcurementPlanItemResponse>
{
    public Guid PlanId { get; set; }

    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public string? PapCode { get; set; }
    public string Description { get; set; } = string.Empty;
    public ProjectType ProjectType { get; set; }

    public int Quantity { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }

    public string Mode { get; set; } = string.Empty;
    public bool IsEarlyProcurement { get; set; }
    public string ScheduleMonth { get; set; } = string.Empty;
    public string FundingSource { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}
