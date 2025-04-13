using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
public sealed record PurchaseItemResponse(Guid? Id, Guid PurchaseId, Guid ProductId, int Qty, decimal UnitPrice, PurchaseStatus? ItemStatus, ProductResponse? Product);
