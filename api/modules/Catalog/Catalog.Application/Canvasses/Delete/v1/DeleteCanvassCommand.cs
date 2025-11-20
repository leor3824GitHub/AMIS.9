using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Delete.v1;

public sealed record DeleteCanvassCommand(Guid Id) : IRequest<DeleteCanvassResponse>;
