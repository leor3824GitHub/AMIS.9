using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Update.v1;

public sealed class UpdateProcurementPlanCommand : IRequest<UpdateProcurementPlanResponse>
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsSupplemental { get; set; }
    public BudgetType BudgetType { get; set; }
}

