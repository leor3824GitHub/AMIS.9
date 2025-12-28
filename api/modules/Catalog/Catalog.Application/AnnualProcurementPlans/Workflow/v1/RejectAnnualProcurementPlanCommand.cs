using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

public sealed record RejectAnnualProcurementPlanCommand(Guid Id, Guid RejectedByUserId, string Reason) : IRequest;
