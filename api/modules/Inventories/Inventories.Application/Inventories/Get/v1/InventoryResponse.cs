using AMIS.WebApi.Inventories.Application.Products.Get.v1;

namespace AMIS.WebApi.Inventories.Application.Inventories.Get.v1;
public sealed record InventoryResponse(Guid? Id, Guid ProductId , int Qty, decimal AvePrice, ProductResponse? Product);

