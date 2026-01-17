using AMIS.WebApi.Inventories.Application.Products.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Purchases.Get.v1;

public sealed record PurchaseItemResponse(
    Guid? Id,
    Guid? ProductId,
    int Qty,
    decimal UnitPrice,
    PurchaseStatus? ItemStatus,
    ProductResponse? Product
);

