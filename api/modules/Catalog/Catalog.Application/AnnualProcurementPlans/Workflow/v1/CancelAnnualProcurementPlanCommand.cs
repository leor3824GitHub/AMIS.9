using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

public sealed record CancelAnnualProcurementPlanCommand(Guid Id) : IRequest;
