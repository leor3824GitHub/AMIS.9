using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Update.v1;

public sealed class UpdateAnnualProcurementPlanCommand : IRequest<UpdateAnnualProcurementPlanResponse>
{
    public Guid Id { get; set; }
    public int FiscalYear { get; set; }
    public BudgetType BudgetType { get; set; }
}
