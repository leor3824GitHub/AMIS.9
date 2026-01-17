using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Items.v1;

public sealed record DeleteAnnualProcurementPlanItemCommand(Guid PlanId, Guid ItemId) : IRequest;

