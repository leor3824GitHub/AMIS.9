using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
public sealed record InventoryResponse(
    Guid? Id, 
    Guid ProductId, 
    int Qty, 
    decimal AvePrice, 
    StockStatus StockStatus,
    int ReservedQty,
    string? Location,
    ProductResponse? Product);
