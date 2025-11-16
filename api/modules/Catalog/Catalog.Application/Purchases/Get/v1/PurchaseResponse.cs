using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

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
