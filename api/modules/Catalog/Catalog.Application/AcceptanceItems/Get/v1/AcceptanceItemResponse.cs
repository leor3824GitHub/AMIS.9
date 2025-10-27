using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public sealed record AcceptanceItemResponse(
    Guid Id,
    Guid AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks,
    PurchaseItemResponse? PurchaseItem
);

