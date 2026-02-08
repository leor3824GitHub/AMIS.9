using System.Collections.ObjectModel;

namespace Shared.Authorization;

public static class FshPermissions
{
    private static readonly FshPermission[] AllPermissions =
    [     
        //tenants
        new("View Tenants", FshActions.View, FshResources.Tenants, IsRoot: true),
        new("Create Tenants", FshActions.Create, FshResources.Tenants, IsRoot: true),
        new("Update Tenants", FshActions.Update, FshResources.Tenants, IsRoot: true),
        new("Upgrade Tenant Subscription", FshActions.UpgradeSubscription, FshResources.Tenants, IsRoot: true),

        //identity
        new("View Users", FshActions.View, FshResources.Users),
        new("Search Users", FshActions.Search, FshResources.Users),
        new("Create Users", FshActions.Create, FshResources.Users),
        new("Update Users", FshActions.Update, FshResources.Users),
        new("Delete Users", FshActions.Delete, FshResources.Users),
        new("Export Users", FshActions.Export, FshResources.Users),
        new("View UserRoles", FshActions.View, FshResources.UserRoles),
        new("Update UserRoles", FshActions.Update, FshResources.UserRoles),
        new("View Roles", FshActions.View, FshResources.Roles),
        new("Create Roles", FshActions.Create, FshResources.Roles),
        new("Update Roles", FshActions.Update, FshResources.Roles),
        new("Delete Roles", FshActions.Delete, FshResources.Roles),
        new("View RoleClaims", FshActions.View, FshResources.RoleClaims),
        new("Update RoleClaims", FshActions.Update, FshResources.RoleClaims),
        
        //products
        new("View Products", FshActions.View, FshResources.Products, IsBasic: true),
        new("Search Products", FshActions.Search, FshResources.Products, IsBasic: true),
        new("Create Products", FshActions.Create, FshResources.Products),
        new("Update Products", FshActions.Update, FshResources.Products),
        new("Delete Products", FshActions.Delete, FshResources.Products),
        new("Export Products", FshActions.Export, FshResources.Products),

        //brands
        new("View Brands", FshActions.View, FshResources.Brands, IsBasic: true),
        new("Search Brands", FshActions.Search, FshResources.Brands, IsBasic: true),
        new("Create Brands", FshActions.Create, FshResources.Brands),
        new("Update Brands", FshActions.Update, FshResources.Brands),
        new("Delete Brands", FshActions.Delete, FshResources.Brands),
        new("Export Brands", FshActions.Export, FshResources.Brands),

        //categories
        new("View Categories", FshActions.View, FshResources.Categories, IsBasic: true),
        new("Search Categories", FshActions.Search, FshResources.Categories, IsBasic: true),
        new("Create Categories", FshActions.Create, FshResources.Categories),
        new("Update Categories", FshActions.Update, FshResources.Categories),
        new("Delete Categories", FshActions.Delete, FshResources.Categories),
        new("Export Categories", FshActions.Export, FshResources.Categories),

        //nfa office codes
        new("View NfaOfficeCodes", FshActions.View, FshResources.NfaOfficeCodes, IsBasic: true),
        new("Search NfaOfficeCodes", FshActions.Search, FshResources.NfaOfficeCodes, IsBasic: true),

        //ppe category codes
        new("View PpeCategoryCodes", FshActions.View, FshResources.PpeCategoryCodes, IsBasic: true),
        new("Search PpeCategoryCodes", FshActions.Search, FshResources.PpeCategoryCodes, IsBasic: true),

        //inventories
        new("View Inventories", FshActions.View, FshResources.Inventories, IsBasic: true),
        new("Search Inventories", FshActions.Search, FshResources.Inventories, IsBasic: true),
        new("Create Inventories", FshActions.Create, FshResources.Inventories),
        new("Update Inventories", FshActions.Update, FshResources.Inventories),
        new("Delete Inventories", FshActions.Delete, FshResources.Inventories),
        new("Export Inventories", FshActions.Export, FshResources.Inventories),

         //suppliers
        new("View Suppliers", FshActions.View, FshResources.Suppliers, IsBasic: true),
        new("Search Suppliers", FshActions.Search, FshResources.Suppliers, IsBasic: true),
        new("Create Suppliers", FshActions.Create, FshResources.Suppliers),
        new("Update Suppliers", FshActions.Update, FshResources.Suppliers),
        new("Delete Suppliers", FshActions.Delete, FshResources.Suppliers),
        new("Export Suppliers", FshActions.Export, FshResources.Suppliers),

        //purchases
        new("View Purchases", FshActions.View, FshResources.Purchases, IsBasic: true),
        new("Search Purchases", FshActions.Search, FshResources.Purchases, IsBasic: true),
        new("Create Purchases", FshActions.Create, FshResources.Purchases),
        new("Update Purchases", FshActions.Update, FshResources.Purchases),
        new("Delete Purchases", FshActions.Delete, FshResources.Purchases),
        new("Export Purchases", FshActions.Export, FshResources.Purchases),

        //purchaseitems
        new("View PurchaseItems", FshActions.View, FshResources.PurchaseItems, IsBasic: true),
        new("Search PurchaseItems", FshActions.Search, FshResources.PurchaseItems, IsBasic: true),
        new("Create PurchaseItems", FshActions.Create, FshResources.PurchaseItems),
        new("Update PurchaseItems", FshActions.Update, FshResources.PurchaseItems),
        new("Delete PurchaseItems", FshActions.Delete, FshResources.PurchaseItems),
        new("Export PurchaseItems", FshActions.Export, FshResources.PurchaseItems),

            //purchaserequests
            new("View PurchaseRequests", FshActions.View, FshResources.PurchaseRequests, IsBasic: true),
            new("Search PurchaseRequests", FshActions.Search, FshResources.PurchaseRequests, IsBasic: true),
            new("Create PurchaseRequests", FshActions.Create, FshResources.PurchaseRequests),
            new("Update PurchaseRequests", FshActions.Update, FshResources.PurchaseRequests),
            new("Delete PurchaseRequests", FshActions.Delete, FshResources.PurchaseRequests),
            new("Export PurchaseRequests", FshActions.Export, FshResources.PurchaseRequests),

            //purchaserequestitems
            new("View PurchaseRequestItems", FshActions.View, FshResources.PurchaseRequestItems, IsBasic: true),
            new("Search PurchaseRequestItems", FshActions.Search, FshResources.PurchaseRequestItems, IsBasic: true),
            new("Create PurchaseRequestItems", FshActions.Create, FshResources.PurchaseRequestItems),
            new("Update PurchaseRequestItems", FshActions.Update, FshResources.PurchaseRequestItems),
            new("Delete PurchaseRequestItems", FshActions.Delete, FshResources.PurchaseRequestItems),
            new("Export PurchaseRequestItems", FshActions.Export, FshResources.PurchaseRequestItems),

            //canvasses
            new("View Canvasses", FshActions.View, FshResources.Canvasses, IsBasic: true),
            new("Search Canvasses", FshActions.Search, FshResources.Canvasses, IsBasic: true),
            new("Create Canvasses", FshActions.Create, FshResources.Canvasses),
            new("Edit Canvasses", FshActions.Update, FshResources.Canvasses),
            new("Delete Canvasses", FshActions.Delete, FshResources.Canvasses),

        //employees
        new("View Employees", FshActions.View, FshResources.Employees, IsBasic: true),
        new("Search Employees", FshActions.Search, FshResources.Employees, IsBasic: true),
        new("Create Employees", FshActions.Create, FshResources.Employees),
        new("Update Employees", FshActions.Update, FshResources.Employees),
        new("Delete Employees", FshActions.Delete, FshResources.Employees),
        new("Export Employees", FshActions.Export, FshResources.Employees),

        //issuances
        new("View Issuances", FshActions.View, FshResources.Issuances, IsBasic: true),
        new("Search Issuances", FshActions.Search, FshResources.Issuances, IsBasic: true),
        new("Create Issuances", FshActions.Create, FshResources.Issuances),
        new("Update Issuances", FshActions.Update, FshResources.Issuances),
        new("Delete Issuances", FshActions.Delete, FshResources.Issuances),
        new("Export Issuances", FshActions.Export, FshResources.Issuances),

        //issuanceitems
        new("View IssuanceItems", FshActions.View, FshResources.IssuanceItems, IsBasic: true),
        new("Search IssuanceItems", FshActions.Search, FshResources.IssuanceItems, IsBasic: true),
        new("Create IssuanceItems", FshActions.Create, FshResources.IssuanceItems),
        new("Update IssuanceItems", FshActions.Update, FshResources.IssuanceItems),
        new("Delete IssuanceItems", FshActions.Delete, FshResources.IssuanceItems),
        new("Export IssuanceItems", FshActions.Export, FshResources.IssuanceItems),

        //supplies and materials issuance reports (SMIR)
        new("View SuppliesAndMaterialsIssuance", FshActions.View, FshResources.SuppliesAndMaterialsIssuance, IsBasic: true),
        new("Search SuppliesAndMaterialsIssuance", FshActions.Search, FshResources.SuppliesAndMaterialsIssuance, IsBasic: true),
        new("Create SuppliesAndMaterialsIssuance", FshActions.Create, FshResources.SuppliesAndMaterialsIssuance),
        new("Update SuppliesAndMaterialsIssuance", FshActions.Update, FshResources.SuppliesAndMaterialsIssuance),
        new("Delete SuppliesAndMaterialsIssuance", FshActions.Delete, FshResources.SuppliesAndMaterialsIssuance),
        new("Cancel SuppliesAndMaterialsIssuance", FshActions.Cancel, FshResources.SuppliesAndMaterialsIssuance),

        //supplies and materials receiving reports (SMRR)
        new("View SuppliesAndMaterialsReceiving", FshActions.View, FshResources.SuppliesAndMaterialsReceiving, IsBasic: true),
        new("Search SuppliesAndMaterialsReceiving", FshActions.Search, FshResources.SuppliesAndMaterialsReceiving, IsBasic: true),
        new("Create SuppliesAndMaterialsReceiving", FshActions.Create, FshResources.SuppliesAndMaterialsReceiving),
        new("Update SuppliesAndMaterialsReceiving", FshActions.Update, FshResources.SuppliesAndMaterialsReceiving),
        new("Delete SuppliesAndMaterialsReceiving", FshActions.Delete, FshResources.SuppliesAndMaterialsReceiving),
        new("Cancel SuppliesAndMaterialsReceiving", FshActions.Cancel, FshResources.SuppliesAndMaterialsReceiving),

        //ppe issuance reports (PPEIR)
        new("View PpeIssuance", FshActions.View, FshResources.PpeIssuance, IsBasic: true),
        new("Search PpeIssuance", FshActions.Search, FshResources.PpeIssuance, IsBasic: true),
        new("Create PpeIssuance", FshActions.Create, FshResources.PpeIssuance),
        new("Update PpeIssuance", FshActions.Update, FshResources.PpeIssuance),
        new("Delete PpeIssuance", FshActions.Delete, FshResources.PpeIssuance),
        new("Post PpeIssuance", FshActions.Post, FshResources.PpeIssuance),
        new("Cancel PpeIssuance", FshActions.Cancel, FshResources.PpeIssuance),

        //ppe receiving reports (PPER)
        new("View Pper", FshActions.View, FshResources.Pper, IsBasic: true),
        new("Search Pper", FshActions.Search, FshResources.Pper, IsBasic: true),
        new("Create Pper", FshActions.Create, FshResources.Pper),
        new("Update Pper", FshActions.Update, FshResources.Pper),
        new("Post Pper", FshActions.Post, FshResources.Pper),
        new("Delete Pper", FshActions.Delete, FshResources.Pper),
        new("Cancel Pper", FshActions.Cancel, FshResources.Pper),

        //property acknowledgement receipt (PAR)
        new("View PropertyAcknowledgementReceipt", FshActions.View, FshResources.PropertyAcknowledgementReceipt, IsBasic: true),
        new("Search PropertyAcknowledgementReceipt", FshActions.Search, FshResources.PropertyAcknowledgementReceipt, IsBasic: true),
        new("Create PropertyAcknowledgementReceipt", FshActions.Create, FshResources.PropertyAcknowledgementReceipt),
        new("Update PropertyAcknowledgementReceipt", FshActions.Update, FshResources.PropertyAcknowledgementReceipt),
        new("Post PropertyAcknowledgementReceipt", FshActions.Post, FshResources.PropertyAcknowledgementReceipt),
        new("Cancel PropertyAcknowledgementReceipt", FshActions.Cancel, FshResources.PropertyAcknowledgementReceipt),
        new("Return PropertyAcknowledgementReceipt", FshActions.Return, FshResources.PropertyAcknowledgementReceipt),

        //inventorytransactions
        new("View InventoryTransactions", FshActions.View, FshResources.InventoryTransactions, IsBasic: true),
        new("Search InventoryTransactions", FshActions.Search, FshResources.InventoryTransactions, IsBasic: true),
        new("Create InventoryTransactions", FshActions.Create, FshResources.InventoryTransactions),
        new("Update InventoryTransactions", FshActions.Update, FshResources.InventoryTransactions),
        new("Delete InventoryTransactions", FshActions.Delete, FshResources.InventoryTransactions),
        new("Export InventoryTransactions", FshActions.Export, FshResources.InventoryTransactions),

        //inpections
        new("View Inspections", FshActions.View, FshResources.Inspections, IsBasic: true),
        new("Search Inspections", FshActions.Search, FshResources.Inspections, IsBasic: true),
        new("Create Inspections", FshActions.Create, FshResources.Inspections),
        new("Update Inspections", FshActions.Update, FshResources.Inspections),
        new("Delete Inspections", FshActions.Delete, FshResources.Inspections),
        new("Export Inspections", FshActions.Export, FshResources.Inspections),
        new("Complete Inspections", FshActions.Complete, FshResources.Inspections),
        // (Approve/Reject/Cancel mapped to Accept/Cancel if needed later)

        //inpectionitems
        new("View InspectionItems", FshActions.View, FshResources.InspectionItems, IsBasic: true),
        new("Search InspectionItems", FshActions.Search, FshResources.InspectionItems, IsBasic: true),
        new("Create InspectionItems", FshActions.Create, FshResources.InspectionItems),
        new("Update InspectionItems", FshActions.Update, FshResources.InspectionItems),
        new("Delete InspectionItems", FshActions.Delete, FshResources.InspectionItems),
        new("Export InspectionItems", FshActions.Export, FshResources.InspectionItems),

        //acceptances
        new("View Acceptances", FshActions.View, FshResources.Acceptances, IsBasic: true),
        new("Search Acceptances", FshActions.Search, FshResources.Acceptances, IsBasic: true),
        new("Create Acceptances", FshActions.Create, FshResources.Acceptances),
        new("Update Acceptances", FshActions.Update, FshResources.Acceptances),
        new("Delete Acceptances", FshActions.Delete, FshResources.Acceptances),
        new("Export Acceptances", FshActions.Export, FshResources.Acceptances),
        new("Post Acceptances", FshActions.Post, FshResources.Acceptances),
        new("Link Acceptances", FshActions.Link, FshResources.Acceptances),
        new("Cancel Acceptances", FshActions.Cancel, FshResources.Acceptances),

        //acceptanceitems
        new("View AcceptanceItems", FshActions.View, FshResources.AcceptanceItems, IsBasic: true),
        new("Search AcceptanceItems", FshActions.Search, FshResources.AcceptanceItems, IsBasic: true),
        new("Create AcceptanceItems", FshActions.Create, FshResources.AcceptanceItems),
        new("Update AcceptanceItems", FshActions.Update, FshResources.AcceptanceItems),
        new("Delete AcceptanceItems", FshActions.Delete, FshResources.AcceptanceItems),
        new("Export AcceptanceItems", FshActions.Export, FshResources.AcceptanceItems),

        //inspectionrequests
        new("View InspectionRequests", FshActions.View, FshResources.InspectionRequests, IsBasic: true),
        new("Search InspectionRequests", FshActions.Search, FshResources.InspectionRequests, IsBasic: true),
        new("Create InspectionRequests", FshActions.Create, FshResources.InspectionRequests),
        new("Update InspectionRequests", FshActions.Update, FshResources.InspectionRequests),
        new("Delete InspectionRequests", FshActions.Delete, FshResources.InspectionRequests),
        new("Export InspectionRequests", FshActions.Export, FshResources.InspectionRequests),
        new("Assign InspectionRequests", FshActions.Assign, FshResources.InspectionRequests),
        new("Complete InspectionRequests", FshActions.Complete, FshResources.InspectionRequests),
        new("Accept InspectionRequests", FshActions.Accept, FshResources.InspectionRequests),
        new("StatusUpdate InspectionRequests", FshActions.StatusUpdate, FshResources.InspectionRequests),

        //asset requisitions
        new("View AssetRequisitions", FshActions.View, FshResources.AssetRequisitions, IsBasic: true),
        new("Create AssetRequisitions", FshActions.Create, FshResources.AssetRequisitions),
        new("Accept AssetRequisitions", FshActions.Accept, FshResources.AssetRequisitions),

        //asset classification rules
        new("View AssetClassificationRules", FshActions.View, FshResources.AssetClassificationRules, IsBasic: true),
        new("Search AssetClassificationRules", FshActions.Search, FshResources.AssetClassificationRules, IsBasic: true),
        new("Create AssetClassificationRules", FshActions.Create, FshResources.AssetClassificationRules),
        new("Update AssetClassificationRules", FshActions.Update, FshResources.AssetClassificationRules),
        new("Delete AssetClassificationRules", FshActions.Delete, FshResources.AssetClassificationRules),
        new("Export AssetClassificationRules", FshActions.Export, FshResources.AssetClassificationRules),

        //physical assets
        new("View PhysicalAssets", FshActions.View, FshResources.PhysicalAssets, IsBasic: true),
        new("Search PhysicalAssets", FshActions.Search, FshResources.PhysicalAssets, IsBasic: true),
        new("Create PhysicalAssets", FshActions.Create, FshResources.PhysicalAssets),
        new("Update PhysicalAssets", FshActions.Update, FshResources.PhysicalAssets),
        new("Delete PhysicalAssets", FshActions.Delete, FshResources.PhysicalAssets),
        new("Export PhysicalAssets", FshActions.Export, FshResources.PhysicalAssets),
        new("Issue PhysicalAssets", FshActions.Issue, FshResources.PhysicalAssets),
        new("Return PhysicalAssets", FshActions.Return, FshResources.PhysicalAssets),

        //property code sequences
        new("View PropertyCodeSequences", FshActions.View, FshResources.PropertyCodeSequences, IsBasic: true),
        new("Search PropertyCodeSequences", FshActions.Search, FshResources.PropertyCodeSequences, IsBasic: true),
        new("Create PropertyCodeSequences", FshActions.Create, FshResources.PropertyCodeSequences),
        new("Update PropertyCodeSequences", FshActions.Update, FshResources.PropertyCodeSequences),
        new("Delete PropertyCodeSequences", FshActions.Delete, FshResources.PropertyCodeSequences),
        new("Export PropertyCodeSequences", FshActions.Export, FshResources.PropertyCodeSequences),

        //depreciation schedules
        new("View DepreciationSchedules", FshActions.View, FshResources.DepreciationSchedules, IsBasic: true),
        new("Create DepreciationSchedules", FshActions.Create, FshResources.DepreciationSchedules),
        new("Post DepreciationSchedules", FshActions.Post, FshResources.DepreciationSchedules),
        new("Reverse DepreciationSchedules", FshActions.Reverse, FshResources.DepreciationSchedules),

        //journal entry vouchers
        new("View JournalEntryVouchers", FshActions.View, FshResources.JournalEntryVouchers, IsBasic: true),
        new("Create JournalEntryVouchers", FshActions.Create, FshResources.JournalEntryVouchers),
        new("Submit JournalEntryVouchers", FshActions.Submit, FshResources.JournalEntryVouchers),
        new("Approve JournalEntryVouchers", FshActions.Approve, FshResources.JournalEntryVouchers),
        new("Post JournalEntryVouchers", FshActions.Post, FshResources.JournalEntryVouchers),

        //ppe type mappings
        new("View PPE Type Mappings", FshActions.View, FshResources.PPETypeMappings, IsBasic: true),
        new("Manage PPE Type Mappings", FshActions.Manage, FshResources.PPETypeMappings),

        //todos
        new("View Todos", FshActions.View, FshResources.Todos, IsBasic: true),
        new("Search Todos", FshActions.Search, FshResources.Todos, IsBasic: true),
        new("Create Todos", FshActions.Create, FshResources.Todos),
        new("Update Todos", FshActions.Update, FshResources.Todos),
        new("Delete Todos", FshActions.Delete, FshResources.Todos),
        new("Export Todos", FshActions.Export, FshResources.Todos),

        //procurement plans (PPMP)
        new("View Procurement Plans", FshActions.View, FshResources.ProcurementPlans, IsBasic: true),
        new("Search Procurement Plans", FshActions.Search, FshResources.ProcurementPlans, IsBasic: true),
        new("Create Procurement Plans", FshActions.Create, FshResources.ProcurementPlans),
        new("Update Procurement Plans", FshActions.Update, FshResources.ProcurementPlans),
        new("Delete Procurement Plans", FshActions.Delete, FshResources.ProcurementPlans),
        new("Submit Procurement Plans", FshActions.Submit, FshResources.ProcurementPlans),
        new("Approve Procurement Plans", FshActions.Approve, FshResources.ProcurementPlans),

        //procurement plan items (PPMP items)
        new("View Procurement Plan Items", FshActions.View, FshResources.ProcurementPlanItems, IsBasic: true),
        new("Search Procurement Plan Items", FshActions.Search, FshResources.ProcurementPlanItems, IsBasic: true),
        new("Create Procurement Plan Items", FshActions.Create, FshResources.ProcurementPlanItems),
        new("Update Procurement Plan Items", FshActions.Update, FshResources.ProcurementPlanItems),
        new("Delete Procurement Plan Items", FshActions.Delete, FshResources.ProcurementPlanItems),

        //annual procurement plans (APP)
        new("View Annual Procurement Plans", FshActions.View, FshResources.AnnualProcurementPlans, IsBasic: true),
        new("Search Annual Procurement Plans", FshActions.Search, FshResources.AnnualProcurementPlans, IsBasic: true),
        new("Create Annual Procurement Plans", FshActions.Create, FshResources.AnnualProcurementPlans),
        new("Update Annual Procurement Plans", FshActions.Update, FshResources.AnnualProcurementPlans),
        new("Delete Annual Procurement Plans", FshActions.Delete, FshResources.AnnualProcurementPlans),
        new("Submit Annual Procurement Plans", FshActions.Submit, FshResources.AnnualProcurementPlans),
        new("Approve Annual Procurement Plans", FshActions.Approve, FshResources.AnnualProcurementPlans),

        //annual procurement plan items (APP items)
        new("View Annual Procurement Plan Items", FshActions.View, FshResources.AnnualProcurementPlanItems, IsBasic: true),
        new("Search Annual Procurement Plan Items", FshActions.Search, FshResources.AnnualProcurementPlanItems, IsBasic: true),
        new("Create Annual Procurement Plan Items", FshActions.Create, FshResources.AnnualProcurementPlanItems),
        new("Update Annual Procurement Plan Items", FshActions.Update, FshResources.AnnualProcurementPlanItems),
        new("Delete Annual Procurement Plan Items", FshActions.Delete, FshResources.AnnualProcurementPlanItems),

        //procurement projects
        new("View Procurement Projects", FshActions.View, FshResources.ProcurementProjects, IsBasic: true),
        new("Search Procurement Projects", FshActions.Search, FshResources.ProcurementProjects, IsBasic: true),
        new("Create Procurement Projects", FshActions.Create, FshResources.ProcurementProjects),
        new("Update Procurement Projects", FshActions.Update, FshResources.ProcurementProjects),
        new("Delete Procurement Projects", FshActions.Delete, FshResources.ProcurementProjects),

         new("View Hangfire", FshActions.View, FshResources.Hangfire),
         new("View Dashboard", FshActions.View, FshResources.Dashboard),

        //audit
        new("View Audit Trails", FshActions.View, FshResources.AuditTrails),
    ];

    public static IReadOnlyList<FshPermission> All { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions);
    public static IReadOnlyList<FshPermission> Root { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Admin { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => !p.IsRoot).ToArray());
    public static IReadOnlyList<FshPermission> Basic { get; } = new ReadOnlyCollection<FshPermission>(AllPermissions.Where(p => p.IsBasic).ToArray());
}

public record FshPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
{
    public string Name => NameFor(Action, Resource);
    public static string NameFor(string action, string resource)
    {
        return $"Permissions.{resource}.{action}";
    }
}


