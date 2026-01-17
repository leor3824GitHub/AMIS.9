using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;

public sealed record RejectAnnualProcurementPlanCommand(Guid Id, Guid RejectedByUserId, string Reason) : IRequest;

