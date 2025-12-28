using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;

public sealed record RevertProcurementPlanToDraftCommand(Guid Id) : IRequest;
