using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;

public sealed record RejectProcurementPlanCommand(Guid Id, Guid RejectedByUserId, string Reason) : IRequest;
