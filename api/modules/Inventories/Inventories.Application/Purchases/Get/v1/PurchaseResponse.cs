using AMIS.WebApi.Inventories.Application.Suppliers.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Purchases.Get.v1;

public sealed record PurchaseResponse(
    Guid? Id,
    Guid? SupplierId,
    DateTime? PurchaseDate,
    decimal TotalAmount,
    PurchaseStatus? Status,
    SupplierResponse? Supplier,
    ICollection<PurchaseItemResponse>? Items,
    string? DeliveryAddress
);

