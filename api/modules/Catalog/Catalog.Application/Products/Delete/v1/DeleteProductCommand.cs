using MediatR;

namespace AMIS.WebApi.Catalog.Application.Products.Delete.v1;
public sealed record DeleteProductCommand(
    Guid Id) : IRequest;
