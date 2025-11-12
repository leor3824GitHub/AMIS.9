namespace AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;

public sealed record UpdatePurchaseWithItemsResponse(
    Guid PurchaseId,
    IReadOnlyList<Guid> ItemIds
);
