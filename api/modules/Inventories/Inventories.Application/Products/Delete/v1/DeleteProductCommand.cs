using MediatR;

namespace AMIS.WebApi.Inventories.Application.Products.Delete.v1;
public sealed record DeleteProductCommand(
    Guid Id) : IRequest;

