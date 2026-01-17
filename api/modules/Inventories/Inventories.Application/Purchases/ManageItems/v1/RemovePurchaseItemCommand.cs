using MediatR;

namespace AMIS.WebApi.Inventories.Application.Purchases.ManageItems.v1;

public sealed record RemovePurchaseItemCommand(Guid PurchaseId, Guid ItemId) : IRequest;

