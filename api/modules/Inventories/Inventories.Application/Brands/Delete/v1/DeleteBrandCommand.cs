using MediatR;

namespace AMIS.WebApi.Inventories.Application.Brands.Delete.v1;
public sealed record DeleteBrandCommand(
    Guid Id) : IRequest;

