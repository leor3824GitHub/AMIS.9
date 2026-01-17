using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Inspections.Get.v1;

public sealed record InspectionItemResponse(
    Guid Id,
    Guid InspectionId,
    Guid PurchaseItemId,
    int QtyInspected,
    int QtyPassed,
    int QtyFailed,
    string? Remarks,
    InspectionItemStatus InspectionItemStatus
);

