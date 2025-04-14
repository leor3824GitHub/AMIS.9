using AMIS.WebApi.Catalog.Application.Purchases.Create.v1;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
public sealed record UpdatePurchaseResponse(Guid? Id, List<PurchaseItemDto> Items);
