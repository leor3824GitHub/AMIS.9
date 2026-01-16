using MediatR;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Get.v1;

public sealed record GetAnnualProcurementPlanRequest(Guid Id) : IRequest<GetAnnualProcurementPlanResponse?>;
