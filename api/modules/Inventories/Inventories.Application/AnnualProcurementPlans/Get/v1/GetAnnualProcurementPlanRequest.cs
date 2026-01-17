using MediatR;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;

public sealed record GetAnnualProcurementPlanRequest(Guid Id) : IRequest<GetAnnualProcurementPlanResponse?>;

