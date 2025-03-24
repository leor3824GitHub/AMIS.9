using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inventories.Delete.v1;
public sealed record DeleteInventoryCommand(
    Guid Id) : IRequest;
