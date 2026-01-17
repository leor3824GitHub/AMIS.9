using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Update.v1;

public sealed class UpdateAnnualProcurementPlanCommand : IRequest<UpdateAnnualProcurementPlanResponse>
{
    public Guid Id { get; set; }
    public int FiscalYear { get; set; }
    public BudgetType BudgetType { get; set; }
}

