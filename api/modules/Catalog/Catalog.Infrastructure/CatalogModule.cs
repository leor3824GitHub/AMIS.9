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
using AMIS.WebApi.Catalog.Application.Products.Create.v1;
using FluentValidation;
using AMIS.WebApi.Catalog.Application.Products.Update.v1;

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
            productGroup.MapProductsDeleteEndpoint();

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

            var supplierGroup = app.MapGroup("suppliers").WithTags("suppliers");
            supplierGroup.MapSupplierCreationEndpoint();
            supplierGroup.MapGetSupplierEndpoint();
            supplierGroup.MapGetSupplierListEndpoint();
            supplierGroup.MapSupplierUpdateEndpoint();
            supplierGroup.MapSupplierDeleteEndpoint();

            var purchaseGroup = app.MapGroup("purchases").WithTags("purchases");
            purchaseGroup.MapPurchaseCreationEndpoint();
            purchaseGroup.MapGetPurchaseEndpoint();
            purchaseGroup.MapGetPurchaseListEndpoint();
            purchaseGroup.MapPurchaseUpdateEndpoint();
            purchaseGroup.MapPurchaseDeleteEndpoint();
            purchaseGroup.MapPurchasesDeleteEndpoint();

            //var purchaseItemGroup = app.MapGroup("purchaseItems").WithTags("purchaseItems");
            //purchaseItemGroup.MapPurchaseItemCreationEndpoint();
            //purchaseItemGroup.MapBulkPurchaseItemCreationEndpoint();
            //purchaseItemGroup.MapGetPurchaseItemEndpoint();
            //purchaseItemGroup.MapGetPurchaseItemListEndpoint();
            //purchaseItemGroup.MapPurchaseItemUpdateEndpoint();
            //purchaseItemGroup.MapPurchaseItemDeleteEndpoint();

            var employeeGroup = app.MapGroup("employees").WithTags("employees");
            employeeGroup.MapEmployeeCreationEndpoint();
            employeeGroup.MapGetEmployeeEndpoint();
            employeeGroup.MapGetEmployeeListEndpoint();
            employeeGroup.MapEmployeeUpdateEndpoint();
            employeeGroup.MapEmployeeDeleteEndpoint();

            var issuanceGroup = app.MapGroup("issuances").WithTags("issuances");
            issuanceGroup.MapIssuanceCreationEndpoint();
            issuanceGroup.MapGetIssuanceEndpoint();
            issuanceGroup.MapGetIssuanceListEndpoint();
            issuanceGroup.MapIssuanceUpdateEndpoint();
            issuanceGroup.MapIssuanceDeleteEndpoint();

            var issuanceItemGroup = app.MapGroup("issuanceItems").WithTags("issuanceItems");
            issuanceItemGroup.MapIssuanceItemCreationEndpoint();
            issuanceItemGroup.MapGetIssuanceItemEndpoint();
            issuanceItemGroup.MapGetIssuanceItemListEndpoint();
            issuanceItemGroup.MapIssuanceItemUpdateEndpoint();
            issuanceItemGroup.MapIssuanceItemDeleteEndpoint();
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

        builder.Services.AddKeyedScoped<IRepository<Supplier>, CatalogRepository<Supplier>>("catalog:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, CatalogRepository<Supplier>>("catalog:suppliers");

        builder.Services.AddKeyedScoped<IRepository<Purchase>, CatalogRepository<Purchase>>("catalog:purchases");
        builder.Services.AddKeyedScoped<IReadRepository<Purchase>, CatalogRepository<Purchase>>("catalog:purchases");

        //builder.Services.AddKeyedScoped<IRepository<PurchaseItem>, CatalogRepository<PurchaseItem>>("catalog:purchaseItems");
        //builder.Services.AddKeyedScoped<IReadRepository<PurchaseItem>, CatalogRepository<PurchaseItem>>("catalog:purchaseItems");

        builder.Services.AddKeyedScoped<IRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");

        builder.Services.AddKeyedScoped<IRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");
        builder.Services.AddKeyedScoped<IReadRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");

        builder.Services.AddKeyedScoped<IRepository<IssuanceItem>, CatalogRepository<IssuanceItem>>("catalog:issuanceItems");
        builder.Services.AddKeyedScoped<IReadRepository<IssuanceItem>, CatalogRepository<IssuanceItem>>("catalog:issuanceItems");

        builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();

        return builder;
    }
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }
}
