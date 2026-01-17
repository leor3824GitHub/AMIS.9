using MediatR;

namespace AMIS.WebApi.Inventories.Application.Inventories.Delete.v1;
public sealed record DeleteInventoryCommand(
    Guid Id) : IRequest;

