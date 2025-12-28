using MediatR;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Get.v1;

public sealed record GetProcurementProjectRequest(Guid Id) : IRequest<GetProcurementProjectResponse>;
