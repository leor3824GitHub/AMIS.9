namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed record CreatePurchaseResponse(Guid PurchaseId, List<PurchaseItemDto> Items);
