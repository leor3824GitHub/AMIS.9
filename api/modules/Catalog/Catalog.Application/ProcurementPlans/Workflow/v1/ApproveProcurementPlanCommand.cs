using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;

public sealed record ApproveProcurementPlanCommand(Guid Id, Guid ApprovedByUserId) : IRequest;
