using Carter;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
using AMIS.WebApi.Catalog.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Infrastructure;
public static class CatalogModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("catalog") { }
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var productGroup = app.MapGroup("products").WithTags("products");
            productGroup.MapProductCreationEndpoint();
            productGroup.MapGetProductEndpoint();
            productGroup.MapGetProductListEndpoint();
            productGroup.MapProductUpdateEndpoint();
            productGroup.MapProductDeleteEndpoint();

            var brandGroup = app.MapGroup("brands").WithTags("brands");
            brandGroup.MapBrandCreationEndpoint();
            brandGroup.MapGetBrandEndpoint();
            brandGroup.MapGetBrandListEndpoint();
            brandGroup.MapBrandUpdateEndpoint();
            brandGroup.MapBrandDeleteEndpoint();

            var categoryGroup = app.MapGroup("categories").WithTags("categories");
            categoryGroup.MapCategoryCreationEndpoint();
            categoryGroup.MapGetCategoryEndpoint();
            categoryGroup.MapGetCategoryListEndpoint();
            categoryGroup.MapCategoryUpdateEndpoint();
            categoryGroup.MapCategoryDeleteEndpoint();

            var inventoryGroup = app.MapGroup("inventories").WithTags("inventories");
            inventoryGroup.MapInventoryCreationEndpoint();
            inventoryGroup.MapGetInventoryEndpoint();
            inventoryGroup.MapGetInventoryListEndpoint();
            inventoryGroup.MapInventoryUpdateEndpoint();
            inventoryGroup.MapInventoryDeleteEndpoint();
        }
    }
    public static WebApplicationBuilder RegisterCatalogServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<CatalogDbContext>();
        builder.Services.AddScoped<IDbInitializer, CatalogDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Product>, CatalogRepository<Product>>("catalog:products");
        builder.Services.AddKeyedScoped<IReadRepository<Product>, CatalogRepository<Product>>("catalog:products");
        builder.Services.AddKeyedScoped<IRepository<Brand>, CatalogRepository<Brand>>("catalog:brands");
        builder.Services.AddKeyedScoped<IReadRepository<Brand>, CatalogRepository<Brand>>("catalog:brands");
        builder.Services.AddKeyedScoped<IRepository<Category>, CatalogRepository<Category>>("catalog:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, CatalogRepository<Category>>("catalog:categories");

        builder.Services.AddKeyedScoped<IRepository<Inventory>, CatalogRepository<Inventory>>("catalog:inventories");
        builder.Services.AddKeyedScoped<IReadRepository<Inventory>, CatalogRepository<Inventory>>("catalog:inventories");
        return builder;
    }
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }
}
