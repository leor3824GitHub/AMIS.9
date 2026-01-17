using AMIS.WebApi.Inventories.Application.Products.Get.v1;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;

public sealed record PurchaseRequestItemResponse(
    Guid? Id,
    Guid? ProductId,
    string? ManualProductName,
    int Qty,
    string Unit,
    string? Description,
    ProductResponse? Product
);

