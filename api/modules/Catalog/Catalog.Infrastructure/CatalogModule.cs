using Carter;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.WebApi.Catalog.Infrastructure.Persistence;
using AMIS.WebApi.Catalog.Infrastructure.Persistence.Repositories;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.Acceptances.Services;
using AMIS.WebApi.Catalog.Domain.Services;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Employee;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementProjects;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;
using AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
using AMIS.WebApi.Catalog.Infrastructure.Middleware;
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
            categoryGroup.MapSearchCategoriesEndpoint();
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

            purchaseGroup.MapPurchaseItemManagementEndpoints();

            var employeeGroup = app.MapGroup("employees").WithTags("employees");
            employeeGroup.MapEmployeeCreationEndpoint();
            employeeGroup.MapGetEmployeeEndpoint();
            employeeGroup.MapGetEmployeeListEndpoint();
            employeeGroup.MapEmployeeUpdateEndpoint();
            employeeGroup.MapEmployeeDeleteEndpoint();
            employeeGroup.MapCheckEmployeeRegistrationEndpoint();
            employeeGroup.MapSelfRegisterEmployeeEndpoint();

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
            inspectionGroup.MapInspectionApproveEndpoint();
            inspectionGroup.MapInspectionRejectEndpoint();
            inspectionGroup.MapInspectionCancelEndpoint();
            inspectionGroup.MapInspectionCompleteEndpoint();

            var acceptanceGroup = app.MapGroup("acceptances").WithTags("acceptances");
            acceptanceGroup.MapAcceptanceCreationEndpoint();
            acceptanceGroup.MapAcceptanceDeletionEndpoint();
            acceptanceGroup.MapGetAcceptanceEndpoint();
            acceptanceGroup.MapGetAcceptanceListEndpoint();
            acceptanceGroup.MapAcceptanceUpdateEndpoint();
            acceptanceGroup.MapAcceptancePostEndpoint();
            acceptanceGroup.MapAcceptanceLinkInspectionEndpoint();
            acceptanceGroup.MapAcceptanceCancelEndpoint();

            acceptanceGroup.MapAcceptanceItemManagementEndpoints();

            var inspectionRequestGroup = app.MapGroup("inspectionRequests").WithTags("inspectionRequests");
            inspectionRequestGroup.MapInspectionRequestCreationEndpoint();
            inspectionRequestGroup.MapInspectionRequestDeletionEndpoint();
            inspectionRequestGroup.MapInspectionRequestDeletionRangeEndpoint();
            inspectionRequestGroup.MapGetInspectionRequestEndpoint();
            inspectionRequestGroup.MapGetInspectionRequestListEndpoint();
            inspectionRequestGroup.MapInspectionRequestUpdateEndpoint();
            inspectionRequestGroup.MapInspectionRequestAssignInspectorEndpoint();
            inspectionRequestGroup.MapInspectionRequestMarkCompletedEndpoint();
            inspectionRequestGroup.MapInspectionRequestMarkAcceptedEndpoint();
            inspectionRequestGroup.MapInspectionRequestUpdateStatusEndpoint();

            var purchaseRequestGroup = app.MapGroup("purchaseRequests").WithTags("purchaseRequests");
            purchaseRequestGroup.MapPurchaseRequestCreationEndpoint();
            purchaseRequestGroup.MapGetPurchaseRequestEndpoint();
            purchaseRequestGroup.MapGetPurchaseRequestListEndpoint();
            purchaseRequestGroup.MapPurchaseRequestSubmitEndpoint();
            purchaseRequestGroup.MapPurchaseRequestApproveEndpoint();
            purchaseRequestGroup.MapPurchaseRequestRejectEndpoint();
            purchaseRequestGroup.MapPurchaseRequestCancelEndpoint();
            purchaseRequestGroup.MapPurchaseRequestItemManagementEndpoints();

            var canvassGroup = app.MapGroup("canvasses").WithTags("canvasses");
            canvassGroup.MapCanvassCreationEndpoint();
            canvassGroup.MapGetCanvassEndpoint();
            canvassGroup.MapSearchCanvassesEndpoint();
            canvassGroup.MapCanvassUpdateEndpoint();
            canvassGroup.MapCanvassDeleteEndpoint();
            canvassGroup.MapCanvassSelectLowestEndpoint();
            canvassGroup.MapAwardCanvassEndpoint();

            var procurementPlanGroup = app.MapGroup("procurementPlans").WithTags("procurementPlans");
            procurementPlanGroup.MapProcurementPlanCreationEndpoint();
            procurementPlanGroup.MapGetProcurementPlanEndpoint();
            procurementPlanGroup.MapSearchProcurementPlansEndpoint();
            procurementPlanGroup.MapUpdateProcurementPlanEndpoint();
            procurementPlanGroup.MapSubmitProcurementPlanEndpoint();
            procurementPlanGroup.MapApproveProcurementPlanEndpoint();
            procurementPlanGroup.MapRejectProcurementPlanEndpoint();
            procurementPlanGroup.MapCancelProcurementPlanEndpoint();
            procurementPlanGroup.MapRevertProcurementPlanToDraftEndpoint();
            procurementPlanGroup.MapProcurementPlanItemsEndpoints();

            var annualProcurementPlanGroup = app.MapGroup("annualProcurementPlans").WithTags("annualProcurementPlans");
            annualProcurementPlanGroup.MapAnnualProcurementPlanCreationEndpoint();
            annualProcurementPlanGroup.MapGetAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapSearchAnnualProcurementPlansEndpoint();
            annualProcurementPlanGroup.MapUpdateAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapSubmitAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapApproveAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapRejectAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapCancelAnnualProcurementPlanEndpoint();
            annualProcurementPlanGroup.MapRevertAnnualProcurementPlanToDraftEndpoint();
            annualProcurementPlanGroup.MapAnnualProcurementPlanItemsEndpoints();

            var procurementProjectGroup = app.MapGroup("procurementProjects").WithTags("procurementProjects");
            procurementProjectGroup.MapProcurementProjectCreationEndpoint();
            procurementProjectGroup.MapGetProcurementProjectEndpoint();
            procurementProjectGroup.MapSearchProcurementProjectsEndpoint();
            procurementProjectGroup.MapUpdateProcurementProjectEndpoint();
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
        // Domain event handlers for PurchaseItem inspection/acceptance live in Catalog.Application (MediatR scans automatically).

        builder.Services.AddKeyedScoped<IRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, CatalogRepository<Employee>>("catalog:employees");

        builder.Services.AddKeyedScoped<IRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");
        builder.Services.AddKeyedScoped<IReadRepository<Issuance>, CatalogRepository<Issuance>>("catalog:issuances");

        builder.Services.AddKeyedScoped<IRepository<IssuanceItem>, CatalogRepository<IssuanceItem>>("catalog:issuanceItems");
        builder.Services.AddKeyedScoped<IReadRepository<IssuanceItem>, CatalogRepository<IssuanceItem>>("catalog:issuanceItems");

        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, CatalogRepository<InventoryTransaction>>("catalog:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, CatalogRepository<InventoryTransaction>>("catalog:inventory-transactions");

        builder.Services.AddKeyedScoped<IRepository<Inspection>, CatalogRepository<Inspection>>("catalog:inspections");
        builder.Services.AddKeyedScoped<IReadRepository<Inspection>, CatalogRepository<Inspection>>("catalog:inspections");

        builder.Services.AddKeyedScoped<IRepository<Acceptance>, CatalogRepository<Acceptance>>("catalog:acceptances");
        builder.Services.AddKeyedScoped<IReadRepository<Acceptance>, CatalogRepository<Acceptance>>("catalog:acceptances");

        builder.Services.AddKeyedScoped<IRepository<PhysicalAsset>, CatalogRepository<PhysicalAsset>>("catalog:physical-assets");
        builder.Services.AddKeyedScoped<IReadRepository<PhysicalAsset>, CatalogRepository<PhysicalAsset>>("catalog:physical-assets");

        builder.Services.AddKeyedScoped<IRepository<InspectionRequest>, CatalogRepository<InspectionRequest>>("catalog:inspectionRequests");
        builder.Services.AddKeyedScoped<IReadRepository<InspectionRequest>, CatalogRepository<InspectionRequest>>("catalog:inspectionRequests");
        builder.Services.AddKeyedScoped<IRepository<PurchaseRequest>, CatalogRepository<PurchaseRequest>>("catalog:purchaseRequests");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseRequest>, CatalogRepository<PurchaseRequest>>("catalog:purchaseRequests");

        builder.Services.AddKeyedScoped<IRepository<Canvass>, CatalogRepository<Canvass>>("catalog:canvasses");
        builder.Services.AddKeyedScoped<IReadRepository<Canvass>, CatalogRepository<Canvass>>("catalog:canvasses");

        builder.Services.AddScoped<IPPETypeAccountMappingRepository, PPETypeAccountMappingRepository>();

        // Register repositories for configuration entities
        builder.Services.AddKeyedScoped<IRepository<AssetConditionConfiguration>, CatalogRepository<AssetConditionConfiguration>>("catalog:assetConditions");
        builder.Services.AddKeyedScoped<IReadRepository<AssetConditionConfiguration>, CatalogRepository<AssetConditionConfiguration>>("catalog:assetConditions");

        builder.Services.AddKeyedScoped<IRepository<PPETypeDefinition>, CatalogRepository<PPETypeDefinition>>("catalog:ppeTypes");
        builder.Services.AddKeyedScoped<IReadRepository<PPETypeDefinition>, CatalogRepository<PPETypeDefinition>>("catalog:ppeTypes");

        builder.Services.AddKeyedScoped<IRepository<UnitOfMeasure>, CatalogRepository<UnitOfMeasure>>("catalog:unitsOfMeasure");
        builder.Services.AddKeyedScoped<IReadRepository<UnitOfMeasure>, CatalogRepository<UnitOfMeasure>>("catalog:unitsOfMeasure");

        builder.Services.AddKeyedScoped<IRepository<AssetClassificationRule>, CatalogRepository<AssetClassificationRule>>("catalog:classificationRules");
        builder.Services.AddKeyedScoped<IReadRepository<AssetClassificationRule>, CatalogRepository<AssetClassificationRule>>("catalog:classificationRules");

        // Asset creation strategies (override in composition root if needed)
        builder.Services.AddScoped<IAssetPropertyCodeGenerator, DefaultAssetPropertyCodeGenerator>();
        builder.Services.AddScoped<IAssetClassificationResolver, DefaultAssetClassificationResolver>();

        // Procurement Planning (PPMP)
        builder.Services.AddKeyedScoped<IRepository<ProcurementPlanHeader>, CatalogRepository<ProcurementPlanHeader>>("catalog:procurementPlans");
        builder.Services.AddKeyedScoped<IReadRepository<ProcurementPlanHeader>, CatalogRepository<ProcurementPlanHeader>>("catalog:procurementPlans");

        // Annual Procurement Planning (APP)
        builder.Services.AddKeyedScoped<IRepository<AnnualProcurementPlanHeader>, CatalogRepository<AnnualProcurementPlanHeader>>("catalog:annualProcurementPlans");
        builder.Services.AddKeyedScoped<IReadRepository<AnnualProcurementPlanHeader>, CatalogRepository<AnnualProcurementPlanHeader>>("catalog:annualProcurementPlans");

        // Procurement Projects
        builder.Services.AddKeyedScoped<IRepository<ProcurementProject>, CatalogRepository<ProcurementProject>>("catalog:procurementProjects");
        builder.Services.AddKeyedScoped<IReadRepository<ProcurementProject>, CatalogRepository<ProcurementProject>>("catalog:procurementProjects");

        return builder;
    }
    public static WebApplication UseCatalogModule(this WebApplication app)
    {
        // Employee registration middleware removed
        return app;
    }
}
