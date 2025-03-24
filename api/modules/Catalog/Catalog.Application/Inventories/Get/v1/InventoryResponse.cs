using AMIS.WebApi.Catalog.Application.Products.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
public sealed record InventoryResponse(Guid? Id, string? Location, decimal Qty, decimal AvePrice, ProductResponse? Product);
