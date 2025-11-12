using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;

// Updated to depend on new PurchaseItemDto instead of deprecated PurchaseItemResponse.
public sealed record InspectionItemResponse(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus? InspectionItemStatus,
    PurchaseItemDto? PurchaseItem
);

