using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Delete.v1;
public sealed record DeletePurchaseItemCommand(
    Guid Id) : IRequest;
