using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
public sealed record PurchaseItemResponse(Guid? Id, Guid PurchaseId, Guid ProductId, decimal Qty, decimal UnitPrice, string Status, PurchaseResponse? Purchase, ProductResponse? Product);
