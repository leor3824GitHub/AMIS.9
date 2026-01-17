namespace AMIS.WebApi.Inventories.Application.AcceptanceItems.Get.v1;

public sealed record AcceptanceItemResponse(
    Guid Id,
    Guid AcceptanceId,
    Guid PurchaseItemId,
    int QtyAccepted,
    string? Remarks
);

