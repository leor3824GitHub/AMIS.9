using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Get.v1;

public sealed record GetCanvassRequest(Guid Id) : IRequest<CanvassResponse>;

