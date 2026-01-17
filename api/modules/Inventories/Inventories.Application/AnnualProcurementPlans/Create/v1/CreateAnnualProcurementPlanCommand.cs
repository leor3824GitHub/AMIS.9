using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Create.v1;

public sealed class CreateAnnualProcurementPlanCommand : IRequest<CreateAnnualProcurementPlanResponse>
{
    public string ControlNumber { get; set; } = string.Empty;
    public int FiscalYear { get; set; }
    public BudgetType BudgetType { get; set; }
    public Guid PreparedByUserId { get; set; }
}

