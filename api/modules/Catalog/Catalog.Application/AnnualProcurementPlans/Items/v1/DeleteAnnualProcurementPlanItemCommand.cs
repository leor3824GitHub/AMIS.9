using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Items.v1;

public sealed record DeleteAnnualProcurementPlanItemCommand(Guid PlanId, Guid ItemId) : IRequest;
