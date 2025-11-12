using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

// New lightweight item DTO internal to purchase querying; replaces former PurchaseItemResponse dependency.
public sealed record PurchaseItemDto(
    Guid? Id,
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    decimal LineTotal,
    PurchaseItemInspectionStatus? InspectionStatus,
    PurchaseItemAcceptanceStatus? AcceptanceStatus,
    int? QtyInspected,
    int? QtyPassed,
    int? QtyFailed,
    int? QtyAccepted
);

public sealed record PurchaseResponse(
    Guid? Id,
    Guid? SupplierId,
    DateTime? PurchaseDate,
    decimal? TotalAmount, // Will be computed from Items server-side; kept nullable for backward compatibility.
    PurchaseStatus? Status,
    SupplierResponse? Supplier,
    IReadOnlyCollection<PurchaseItemDto>? Items,
    string? ReferenceNumber,
    DateTime? CreatedOn,
    string? Notes,
    string? Currency
);
