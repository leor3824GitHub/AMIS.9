using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Get.v1;

public sealed record GetProcurementPlanRequest(Guid Id) : IRequest<GetProcurementPlanResponse?>;

