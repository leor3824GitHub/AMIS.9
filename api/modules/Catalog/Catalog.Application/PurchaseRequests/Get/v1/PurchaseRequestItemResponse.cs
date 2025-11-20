using AMIS.WebApi.Catalog.Application.Products.Get.v1;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;

public sealed record PurchaseRequestItemResponse(
    Guid? Id,
    Guid? ProductId,
    int Qty,
    string Unit,
    string? Description,
    ProductResponse? Product
);
