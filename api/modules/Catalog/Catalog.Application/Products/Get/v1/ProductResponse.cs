using AMIS.WebApi.Catalog.Application.Categories.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Products.Get.v1;
public sealed record ProductResponse(Guid? Id, string Name, string? Description, decimal Sku, string Location, string Unit, string? ImagePath, CategoryResponse? Category);
