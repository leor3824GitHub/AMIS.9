using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Workflow.v1;

public sealed record CancelProcurementPlanCommand(Guid Id) : IRequest;

