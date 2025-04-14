namespace AMIS.WebApi.Catalog.Application.PurchaseItems.CreateBulk.v1;
public sealed record CreateBulkPurchaseItemResponse(
    List<Guid> PurchaseItemIds
);
