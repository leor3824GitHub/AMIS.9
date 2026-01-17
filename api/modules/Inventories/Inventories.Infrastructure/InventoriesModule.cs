using Carter;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.WebApi.Inventories.Infrastructure.Persistence;
using AMIS.WebApi.Inventories.Infrastructure.Persistence.Repositories;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Application.Acceptances.Services;
using AMIS.WebApi.Inventories.Domain.Services;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Canvass;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Employee;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementProjects;
using AMIS.WebApi.Inventories.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Inventory;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Purchase;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementPlans;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.IssuanceItem;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Acceptance;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryTransaction;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InspectionRequest;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Inspection;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PurchaseRequest;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Product;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Brand;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Category;
using AMIS.Catalog.Infrastructure.Issuances.Features.Accept.v1;
using AMIS.Catalog.Infrastructure.Issuances.Features.Reject.v1;
using AMIS.Catalog.Infrastructure.Issuances.Features.Cancel.v1;
using AMIS.Catalog.Infrastructure.Issuances.Features.Return.v1;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Issuance;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Supplier;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetRequisition;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsIssuance;
using AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SuppliesAndMaterialsReceiving;

namespace AMIS.WebApi.Inventories.Infrastructure;

public static class InventoriesModule
{
    public class Endpoints : CarterModule
    {
        public Endpoints() : base("inventories") { }
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
            issuanceGroup.MapAcceptIssuanceEndpoint();
            issuanceGroup.MapRejectIssuanceEndpoint();
            issuanceGroup.MapCancelIssuanceEndpoint();
            issuanceGroup.MapReturnIssuanceEndpoint();

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

            var assetRequisitionGroup = app.MapGroup("assetRequisitions").WithTags("assetRequisitions");
            assetRequisitionGroup.MapAssetRequisitionCreationEndpoint();
            assetRequisitionGroup.MapAssetRequisitionAcceptanceEndpoint();
            assetRequisitionGroup.MapRejectAssetRequisitionEndpoint();
            assetRequisitionGroup.MapCancelAssetRequisitionEndpoint();
            assetRequisitionGroup.MapSearchAssetRequisitionsEndpoint();
            assetRequisitionGroup.MapGetAssetRequisitionEndpoint();
            assetRequisitionGroup.MapUpdateAssetRequisitionEndpoint();
            assetRequisitionGroup.MapDeleteAssetRequisitionEndpoint();

            var depreciationScheduleGroup = app.MapGroup("depreciationSchedules").WithTags("depreciationSchedules");
            depreciationScheduleGroup.MapDepreciationScheduleCreationEndpoint();
            depreciationScheduleGroup.MapDepreciationSchedulePostingEndpoint();
            depreciationScheduleGroup.MapDepreciationScheduleReverseEndpoint();
            depreciationScheduleGroup.MapSearchDepreciationSchedulesEndpoint();
            depreciationScheduleGroup.MapGetDepreciationScheduleEndpoint();
            depreciationScheduleGroup.MapUpdateDepreciationScheduleEndpoint();
            depreciationScheduleGroup.MapDeleteDepreciationScheduleEndpoint();

            var journalEntryVoucherGroup = app.MapGroup("journalEntryVouchers").WithTags("journalEntryVouchers");
            journalEntryVoucherGroup.MapJournalEntryVoucherCreationEndpoint();
            journalEntryVoucherGroup.MapJournalEntryVoucherPostingEndpoint();
            journalEntryVoucherGroup.MapJournalEntryVoucherSubmitEndpoint();
            journalEntryVoucherGroup.MapJournalEntryVoucherApproveEndpoint();
            journalEntryVoucherGroup.MapJournalEntryVoucherRejectEndpoint();
            journalEntryVoucherGroup.MapExportJournalEntryVoucherEndpoint();
            journalEntryVoucherGroup.MapSearchJournalEntryVouchersEndpoint();
            journalEntryVoucherGroup.MapGetJournalEntryVoucherEndpoint();
            journalEntryVoucherGroup.MapUpdateJournalEntryVoucherEndpoint();
            journalEntryVoucherGroup.MapDeleteJournalEntryVoucherEndpoint();

            var physicalAssetGroup = app.MapGroup("physicalAssets").WithTags("physicalAssets");
            physicalAssetGroup.MapCreatePhysicalAssetEndpoint();
            physicalAssetGroup.MapSearchPhysicalAssetsEndpoint();
            physicalAssetGroup.MapGetPhysicalAssetEndpoint();
            physicalAssetGroup.MapUpdatePhysicalAssetEndpoint();
            physicalAssetGroup.MapDeletePhysicalAssetEndpoint();
            physicalAssetGroup.MapGenerateQRCodeEndpoint();
            physicalAssetGroup.MapAssignCustodianEndpoint();
            physicalAssetGroup.MapUpdateConditionEndpoint();
            physicalAssetGroup.MapRecordDepreciationEndpoint();
            physicalAssetGroup.MapPhysicalAssetIssueEndpoint();
            physicalAssetGroup.MapPhysicalAssetReturnEndpoint();
            physicalAssetGroup.MapExportPhysicalAssetsEndpoint();
            physicalAssetGroup.MapGetStockLevelsEndpoint();

            var suppliesAndMaterialsIssuanceGroup = app.MapGroup("supplies-materials-issuance").WithTags("supplies-materials-issuance");
            suppliesAndMaterialsIssuanceGroup.MapCreateSuppliesAndMaterialsIssuanceReportEndpoint();
            suppliesAndMaterialsIssuanceGroup.MapGetSuppliesAndMaterialsIssuanceReportEndpoint();

            var suppliesAndMaterialsReceivingGroup = app.MapGroup("supplies-materials-receiving").WithTags("supplies-materials-receiving");
            suppliesAndMaterialsReceivingGroup.MapCreateSuppliesAndMaterialsReceivingReportEndpoint();
            suppliesAndMaterialsReceivingGroup.MapGetSuppliesAndMaterialsReceivingReportEndpoint();

            var ppeIssuanceGroup = app.MapGroup("ppe-issuance").WithTags("ppe-issuance");
            ppeIssuanceGroup.MapCreatePpeIssuanceReportEndpoint();
            ppeIssuanceGroup.MapGetPpeIssuanceReportEndpoint();

            var ppeReceivingGroup = app.MapGroup("ppe-receiving").WithTags("ppe-receiving");
            ppeReceivingGroup.MapCreatePpeReceivingReportEndpoint();
            ppeReceivingGroup.MapGetPpeReceivingReportEndpoint();        }
    }
    public static WebApplicationBuilder RegisterInventoriesServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<InventoriesDbContext>();
        builder.Services.AddScoped<IDbInitializer, InventoriesDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<Product>, InventoriesRepository<Product>>("inventories:products");
        builder.Services.AddKeyedScoped<IReadRepository<Product>, InventoriesRepository<Product>>("inventories:products");
        builder.Services.AddKeyedScoped<IRepository<Brand>, InventoriesRepository<Brand>>("inventories:brands");
        builder.Services.AddKeyedScoped<IReadRepository<Brand>, InventoriesRepository<Brand>>("inventories:brands");
        builder.Services.AddKeyedScoped<IRepository<Category>, InventoriesRepository<Category>>("inventories:categories");
        builder.Services.AddKeyedScoped<IReadRepository<Category>, InventoriesRepository<Category>>("inventories:categories");

        builder.Services.AddKeyedScoped<IRepository<Inventory>, InventoriesRepository<Inventory>>("inventories:inventories");
        builder.Services.AddKeyedScoped<IReadRepository<Inventory>, InventoriesRepository<Inventory>>("inventories:inventories");

        builder.Services.AddKeyedScoped<IRepository<Supplier>, InventoriesRepository<Supplier>>("inventories:suppliers");
        builder.Services.AddKeyedScoped<IReadRepository<Supplier>, InventoriesRepository<Supplier>>("inventories:suppliers");

        builder.Services.AddKeyedScoped<IRepository<Purchase>, InventoriesRepository<Purchase>>("inventories:purchases");
        builder.Services.AddKeyedScoped<IReadRepository<Purchase>, InventoriesRepository<Purchase>>("inventories:purchases");
        // Domain event handlers for PurchaseItem inspection/acceptance live in Inventories.Application (MediatR scans automatically).

        builder.Services.AddKeyedScoped<IRepository<Employee>, InventoriesRepository<Employee>>("inventories:employees");
        builder.Services.AddKeyedScoped<IReadRepository<Employee>, InventoriesRepository<Employee>>("inventories:employees");

        builder.Services.AddKeyedScoped<IRepository<Issuance>, InventoriesRepository<Issuance>>("inventories:issuances");
        builder.Services.AddKeyedScoped<IReadRepository<Issuance>, InventoriesRepository<Issuance>>("inventories:issuances");

        builder.Services.AddKeyedScoped<IRepository<IssuanceItem>, InventoriesRepository<IssuanceItem>>("inventories:issuanceItems");
        builder.Services.AddKeyedScoped<IReadRepository<IssuanceItem>, InventoriesRepository<IssuanceItem>>("inventories:issuanceItems");

        builder.Services.AddKeyedScoped<IRepository<InventoryTransaction>, InventoriesRepository<InventoryTransaction>>("inventories:inventory-transactions");
        builder.Services.AddKeyedScoped<IReadRepository<InventoryTransaction>, InventoriesRepository<InventoryTransaction>>("inventories:inventory-transactions");

        builder.Services.AddKeyedScoped<IRepository<Inspection>, InventoriesRepository<Inspection>>("inventories:inspections");
        builder.Services.AddKeyedScoped<IReadRepository<Inspection>, InventoriesRepository<Inspection>>("inventories:inspections");

        builder.Services.AddKeyedScoped<IRepository<Acceptance>, InventoriesRepository<Acceptance>>("inventories:acceptances");
        builder.Services.AddKeyedScoped<IReadRepository<Acceptance>, InventoriesRepository<Acceptance>>("inventories:acceptances");

        builder.Services.AddKeyedScoped<IRepository<PhysicalAsset>, InventoriesRepository<PhysicalAsset>>("inventories:physicalassets");
        builder.Services.AddKeyedScoped<IReadRepository<PhysicalAsset>, InventoriesRepository<PhysicalAsset>>("inventories:physicalassets");

        builder.Services.AddKeyedScoped<IRepository<InspectionRequest>, InventoriesRepository<InspectionRequest>>("inventories:inspectionRequests");
        builder.Services.AddKeyedScoped<IReadRepository<InspectionRequest>, InventoriesRepository<InspectionRequest>>("inventories:inspectionRequests");
        builder.Services.AddKeyedScoped<IRepository<PurchaseRequest>, InventoriesRepository<PurchaseRequest>>("inventories:purchaseRequests");
        builder.Services.AddKeyedScoped<IReadRepository<PurchaseRequest>, InventoriesRepository<PurchaseRequest>>("inventories:purchaseRequests");

        builder.Services.AddKeyedScoped<IRepository<Canvass>, InventoriesRepository<Canvass>>("inventories:canvasses");
        builder.Services.AddKeyedScoped<IReadRepository<Canvass>, InventoriesRepository<Canvass>>("inventories:canvasses");

        builder.Services.AddScoped<IPPETypeAccountMappingRepository, PPETypeAccountMappingRepository>();

        // Register repositories for configuration entities
        builder.Services.AddKeyedScoped<IRepository<AssetConditionConfiguration>, InventoriesRepository<AssetConditionConfiguration>>("inventories:assetConditions");
        builder.Services.AddKeyedScoped<IReadRepository<AssetConditionConfiguration>, InventoriesRepository<AssetConditionConfiguration>>("inventories:assetConditions");

        builder.Services.AddKeyedScoped<IRepository<PPETypeDefinition>, InventoriesRepository<PPETypeDefinition>>("inventories:ppeTypes");
        builder.Services.AddKeyedScoped<IReadRepository<PPETypeDefinition>, InventoriesRepository<PPETypeDefinition>>("inventories:ppeTypes");

        builder.Services.AddKeyedScoped<IRepository<UnitOfMeasure>, InventoriesRepository<UnitOfMeasure>>("inventories:unitsOfMeasure");
        builder.Services.AddKeyedScoped<IReadRepository<UnitOfMeasure>, InventoriesRepository<UnitOfMeasure>>("inventories:unitsOfMeasure");

        builder.Services.AddKeyedScoped<IRepository<AssetClassificationRule>, InventoriesRepository<AssetClassificationRule>>("inventories:classificationRules");
        builder.Services.AddKeyedScoped<IReadRepository<AssetClassificationRule>, InventoriesRepository<AssetClassificationRule>>("inventories:classificationRules");

        // Asset creation strategies (override in composition root if needed)
        builder.Services.AddScoped<IAssetPropertyCodeGenerator, DefaultAssetPropertyCodeGenerator>();
        builder.Services.AddScoped<IAssetClassificationResolver, DefaultAssetClassificationResolver>();

        // Procurement Planning (PPMP)
        builder.Services.AddKeyedScoped<IRepository<ProcurementPlanHeader>, InventoriesRepository<ProcurementPlanHeader>>("inventories:procurementPlans");
        builder.Services.AddKeyedScoped<IReadRepository<ProcurementPlanHeader>, InventoriesRepository<ProcurementPlanHeader>>("inventories:procurementPlans");

        // Annual Procurement Planning (APP)
        builder.Services.AddKeyedScoped<IRepository<AnnualProcurementPlanHeader>, InventoriesRepository<AnnualProcurementPlanHeader>>("inventories:annualProcurementPlans");
        builder.Services.AddKeyedScoped<IReadRepository<AnnualProcurementPlanHeader>, InventoriesRepository<AnnualProcurementPlanHeader>>("inventories:annualProcurementPlans");

        // Procurement Projects
        builder.Services.AddKeyedScoped<IRepository<ProcurementProject>, InventoriesRepository<ProcurementProject>>("inventories:procurementProjects");
        builder.Services.AddKeyedScoped<IReadRepository<ProcurementProject>, InventoriesRepository<ProcurementProject>>("inventories:procurementProjects");

        // Asset Requisitions
        builder.Services.AddKeyedScoped<IRepository<AssetRequisition>, InventoriesRepository<AssetRequisition>>("inventories:assetrequisitions");
        builder.Services.AddKeyedScoped<IReadRepository<AssetRequisition>, InventoriesRepository<AssetRequisition>>("inventories:assetrequisitions");

        // Depreciation Schedules
        builder.Services.AddKeyedScoped<IRepository<DepreciationSchedule>, InventoriesRepository<DepreciationSchedule>>("inventories:depreciationschedules");
        builder.Services.AddKeyedScoped<IReadRepository<DepreciationSchedule>, InventoriesRepository<DepreciationSchedule>>("inventories:depreciationschedules");

        // Journal Entry Vouchers
        builder.Services.AddKeyedScoped<IRepository<JournalEntryVoucher>, InventoriesRepository<JournalEntryVoucher>>("inventories:journalentryvouchers");
        builder.Services.AddKeyedScoped<IReadRepository<JournalEntryVoucher>, InventoriesRepository<JournalEntryVoucher>>("inventories:journalentryvouchers");

        // Supplies and Materials Issuance Report (SMIR)
        builder.Services.AddKeyedScoped<IRepository<SuppliesAndMaterialsIssuanceReport>, InventoriesRepository<SuppliesAndMaterialsIssuanceReport>>("inventories:smir");
        builder.Services.AddKeyedScoped<IReadRepository<SuppliesAndMaterialsIssuanceReport>, InventoriesRepository<SuppliesAndMaterialsIssuanceReport>>("inventories:smir");

        // Supplies and Materials Receiving Report (SMRR)
        builder.Services.AddKeyedScoped<IRepository<SuppliesAndMaterialsReceivingReport>, InventoriesRepository<SuppliesAndMaterialsReceivingReport>>("inventories:smrr");
        builder.Services.AddKeyedScoped<IReadRepository<SuppliesAndMaterialsReceivingReport>, InventoriesRepository<SuppliesAndMaterialsReceivingReport>>("inventories:smrr");

        // PPE Issuance Report (PPEIR)
        builder.Services.AddKeyedScoped<IRepository<PpeIssuanceReport>, InventoriesRepository<PpeIssuanceReport>>("inventories:ppeir");
        builder.Services.AddKeyedScoped<IReadRepository<PpeIssuanceReport>, InventoriesRepository<PpeIssuanceReport>>("inventories:ppeir");

        // PPE Receiving Report (PPERR)
        builder.Services.AddKeyedScoped<IRepository<PpeReceivingReport>, InventoriesRepository<PpeReceivingReport>>("inventories:pperr");
        builder.Services.AddKeyedScoped<IReadRepository<PpeReceivingReport>, InventoriesRepository<PpeReceivingReport>>("inventories:pperr");

        return builder;
    }
    public static WebApplication UseInventoriesModule(this WebApplication app)
    {
        // Employee registration middleware removed
        return app;
    }
}


