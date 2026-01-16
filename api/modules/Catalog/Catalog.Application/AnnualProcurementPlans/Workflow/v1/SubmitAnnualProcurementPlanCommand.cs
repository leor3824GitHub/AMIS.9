using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

public sealed record SubmitAnnualProcurementPlanCommand(Guid Id) : IRequest;
