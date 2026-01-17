using MediatR;

namespace AMIS.WebApi.Inventories.Application.Purchases.Delete.v1;
public sealed record DeletePurchaseCommand(
    Guid Id) : IRequest;

