using Carter;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
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
            inventoryGroup.MapReserveStockEndpoint();
            inventoryGroup.MapQuarantineInventoryEndpoint();
            inventoryGroup.MapReleaseReservationEndpoint();
            inventoryGroup.MapRecordCycleCountEndpoint();
            // Batch 3 Inventory workflow endpoints
            inventoryGroup.MapReleaseFromQuarantineEndpoint();
            inventoryGroup.MapMarkAsDamagedEndpoint();
            inventoryGroup.MapMarkAsObsoleteEndpoint();
            // Batch 4 Inventory workflow endpoints
            inventoryGroup.MapAllocateToProductionEndpoint();
            inventoryGroup.MapSetLocationEndpoint();
            // Batch 5 Inventory workflow endpoints
            inventoryGroup.MapSetCostingMethodEndpoint();

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
            purchaseGroup.MapUpdatePurchaseWithItemsEndpoint();
            purchaseGroup.MapPurchaseDeleteEndpoint();
            purchaseGroup.MapPurchasesDeleteEndpoint();
            purchaseGroup.MapSubmitPurchaseForApprovalEndpoint();
            purchaseGroup.MapApprovePurchaseEndpoint();
            purchaseGroup.MapRejectPurchaseEndpoint();
            purchaseGroup.MapMarkShippedEndpoint();
            purchaseGroup.MapMarkFullyReceivedEndpoint();
            purchaseGroup.MapMarkInvoicedEndpoint();
            purchaseGroup.MapCancelPurchaseEndpoint();
            // Batch 3 Purchase workflow endpoints
            purchaseGroup.MapAcknowledgePurchaseEndpoint();
            purchaseGroup.MapMarkInProgressEndpoint();
            purchaseGroup.MapMarkPartiallyReceivedEndpoint();
            purchaseGroup.MapMarkPendingPaymentEndpoint();
            // Batch 4 Purchase workflow endpoints
            purchaseGroup.MapMarkPendingInvoiceEndpoint();
            purchaseGroup.MapMarkClosedEndpoint();
            purchaseGroup.MapPutPurchaseOnHoldEndpoint();
            purchaseGroup.MapReleasePurchaseFromHoldEndpoint();

            // PurchaseItem endpoints removed; manage items via Purchase aggregate update endpoint

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
            issuanceGroup.MapUpdateIssuanceWithItemsEndpoint();
            issuanceGroup.MapIssuanceDeleteEndpoint();

            var inventoryTranscationGroup = app.MapGroup("inventoryTranscation").WithTags("inventoryTranscation");
            inventoryTranscationGroup.MapInventoryTransactionCreationEndpoint();
            inventoryTranscationGroup.MapInventoryTransactionDeleteEndpoint();
            inventoryTranscationGroup.MapGetInventoryTransactionEndpoint();
            inventoryTranscationGroup.MapGetInventoryTransactionListEndpoint();
            inventoryTranscationGroup.MapInventoryTransactionUpdateEndpoint();

            var inspectionGroup = app.MapGroup("inspection").WithTags("inspection");
            inspectionGroup.MapInspectionCreationEndpoint();
            inspectionGroup.MapInspectionDeletionEndpoint();
            inspectionGroup.MapGetInspectionEndpoint();
            inspectionGroup.MapGetInspectionListEndpoint();
            inspectionGroup.MapInspectionUpdateEndpoint();
            inspectionGroup.MapUpdateInspectionWithItemsEndpoint();
            inspectionGroup.MapInspectionApproveEndpoint();
            inspectionGroup.MapScheduleInspectionEndpoint();
            inspectionGroup.MapQuarantineInspectionEndpoint();
            // Batch 3 Inspection workflow endpoints
            inspectionGroup.MapConditionallyApproveEndpoint();
            inspectionGroup.MapRequireReInspectionEndpoint();
            // Batch 4 Inspection workflow endpoints
            inspectionGroup.MapPutInspectionOnHoldEndpoint();
            inspectionGroup.MapReleaseInspectionFromHoldEndpoint();
            // Batch 5 Inspection workflow endpoints
            inspectionGroup.MapPartiallyApproveEndpoint();
            inspectionGroup.MapCompleteInspectionEndpoint();
            inspectionGroup.MapReleaseInspectionFromQuarantineEndpoint();

            var acceptanceGroup = app.MapGroup("acceptances").WithTags("acceptances");
            acceptanceGroup.MapAcceptanceCreationEndpoint();
            acceptanceGroup.MapAcceptanceDeletionEndpoint();
            acceptanceGroup.MapGetAcceptanceEndpoint();
            acceptanceGroup.MapGetAcceptanceListEndpoint();
            acceptanceGroup.MapAcceptanceUpdateEndpoint();
            acceptanceGroup.MapUpdateAcceptanceWithItemsEndpoint();

            var inspectionRequestGroup = app.MapGroup("inspectionRequests").WithTags("inspectionRequests");
            inspectionRequestGroup.MapInspectionRequestCreationEndpoint();
            inspectionRequestGroup.MapInspectionRequestDeletionEndpoint();
            inspectionRequestGroup.MapInspectionRequestDeletionRangeEndpoint();
            inspectionRequestGroup.MapGetInspectionRequestEndpoint();
            inspectionRequestGroup.MapGetInspectionRequestListEndpoint();
            inspectionRequestGroup.MapInspectionRequestUpdateEndpoint();
            // Batch 5 InspectionRequest workflow endpoints
            inspectionRequestGroup.MapMarkCompletedEndpoint();
            inspectionRequestGroup.MapMarkAcceptedEndpoint();
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

        // Do not expose repositories for PurchaseItem; enforce aggregate boundary via Purchase

        builder.Services.AddKeyedScoped<IRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");

        builder.Services.AddKeyedScoped<IRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");
        builder.Services.AddKeyedScoped<IReadRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");

        // Do not expose repositories for IssuanceItem; enforce aggregate boundary via Issuance

        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, CatalogRepository<InventoryTransaction>>("catalog:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, CatalogRepository<InventoryTransaction>>("catalog:inventory-transactions");

        builder.Services.AddKeyedScoped<IRepository<Inspection>, CatalogRepository<Inspection>>("catalog:inspections");
        builder.Services.AddKeyedScoped<IReadRepository<Inspection>, CatalogRepository<Inspection>>("catalog:inspections");

        builder.Services.AddKeyedScoped<IRepository<Acceptance>, CatalogRepository<Acceptance>>("catalog:acceptances");
        builder.Services.AddKeyedScoped<IReadRepository<Acceptance>, CatalogRepository<Acceptance>>("catalog:acceptances");

        builder.Services.AddKeyedScoped<IRepository<InspectionRequest>, CatalogRepository<InspectionRequest>>("catalog:inspectionRequests");
        builder.Services.AddKeyedScoped<IReadRepository<InspectionRequest>, CatalogRepository<InspectionRequest>>("catalog:inspectionRequests");
        return builder;
    }
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        return app;
    }
}
