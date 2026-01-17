using AMIS.WebApi.Inventories.Application.Categories.Get.v1;

namespace AMIS.WebApi.Inventories.Application.Products.Get.v1;
public sealed record ProductResponse(
    Guid? Id, 
    string Name, 
    string? 
    Description, 
    decimal Sku, 
    string Unit, 
    string? ImagePath,
    Guid? CategoryId,
    CategoryResponse? Category);

