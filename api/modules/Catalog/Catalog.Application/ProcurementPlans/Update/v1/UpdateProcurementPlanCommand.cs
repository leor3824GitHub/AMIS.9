using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Update.v1;

public sealed class UpdateProcurementPlanCommand : IRequest<UpdateProcurementPlanResponse>
{
    public Guid Id { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public bool IsSupplemental { get; set; }
    public BudgetType BudgetType { get; set; }
}
