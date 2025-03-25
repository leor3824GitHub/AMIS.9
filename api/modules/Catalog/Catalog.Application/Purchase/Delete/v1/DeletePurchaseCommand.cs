using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
public sealed record DeletePurchaseCommand(
    Guid Id) : IRequest;
