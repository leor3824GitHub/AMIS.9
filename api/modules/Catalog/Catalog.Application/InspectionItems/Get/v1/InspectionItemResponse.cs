using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;

public sealed record InspectionItemResponse(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    PurchaseItemResponse? PurchaseItem
);

