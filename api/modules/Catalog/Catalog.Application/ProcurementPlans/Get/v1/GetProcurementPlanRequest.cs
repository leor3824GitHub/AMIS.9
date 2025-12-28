using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;

public sealed record GetProcurementPlanRequest(Guid Id) : IRequest<GetProcurementPlanResponse?>;
