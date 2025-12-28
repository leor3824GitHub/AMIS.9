using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Items.v1;

public sealed record DeleteProcurementPlanItemCommand(Guid PlanId, Guid ItemId) : IRequest;
