using MediatR;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Get.v1;

public sealed record GetProcurementProjectRequest(Guid Id) : IRequest<GetProcurementProjectResponse>;

