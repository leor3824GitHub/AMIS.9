using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v1;

public sealed record PurchaseItemUpdateDto(
    Guid? Id,                 // The ID of the purchase item (could be null for new items)
    Guid? PurchaseId,
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus
);
