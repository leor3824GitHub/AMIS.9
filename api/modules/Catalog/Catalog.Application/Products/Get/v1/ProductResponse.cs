using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Products.Get.v1;
public sealed record ProductResponse(
    Guid? Id, 
    string Name, 
    string? 
    Description, 
    decimal Sku, 
    UnitOfMeasure Unit, 
    string? ImagePath,
    Guid? CategoryId,
    CategoryResponse? Category);
