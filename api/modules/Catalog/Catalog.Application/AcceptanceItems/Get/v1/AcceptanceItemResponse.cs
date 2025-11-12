using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public sealed record AcceptanceItemResponse(
    Guid Id,
    Guid AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks,
    PurchaseItemDto? PurchaseItem
);

