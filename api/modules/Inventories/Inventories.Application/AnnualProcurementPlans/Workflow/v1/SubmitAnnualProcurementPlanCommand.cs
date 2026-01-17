using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;

public sealed record SubmitAnnualProcurementPlanCommand(Guid Id) : IRequest;

