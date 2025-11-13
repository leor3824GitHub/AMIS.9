using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;

public sealed record RemovePurchaseItemCommand(Guid PurchaseId, Guid ItemId) : IRequest;
