using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Create.v1;

public sealed class CreateProcurementPlanCommand : IRequest<CreateProcurementPlanResponse>
{
    public string ControlNumber { get; set; } = string.Empty;
    public int FiscalYear { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public bool IsSupplemental { get; set; }
    public BudgetType BudgetType { get; set; }

    public Guid PreparedByUserId { get; set; }
}

