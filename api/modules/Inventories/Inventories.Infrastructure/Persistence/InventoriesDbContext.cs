using Finbuckle.MultiTenant.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.Framework.Infrastructure.Tenant;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence;

public sealed class InventoriesDbContext : FshDbContext
{
    public InventoriesDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<InventoriesDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Inventory> Inventories { get; set; } = null!;
    public DbSet<Supplier> Suppliers { get; set; } = null!;
    public DbSet<Purchase> Purchases { get; set; } = null!;
    public DbSet<PurchaseItem> PurchaseItems { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Issuance> Issuances { get; set; } = null!;
    public DbSet<IssuanceItem> IssuanceItems { get; set; } = null!;
    public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;
    public DbSet<Inspection> Inspections { get; set; } = null!;
    public DbSet<InspectionItem> InspectionItems { get; set; } = null!;
    public DbSet<Acceptance> Acceptances { get; set; } = null!;
    public DbSet<AcceptanceItem> AcceptanceItems { get; set; } = null!;
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; } = null!;
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; } = null!;
    public DbSet<Canvass> Canvasses { get; set; } = null!;

    // COA-Compliant Inventory Tables (Separate per Classification)
    public DbSet<ConsumableInventory> ConsumableInventories { get; set; } = null!;
    // Removed legacy separated tables in favor of unified PhysicalAsset

    // Unified Physical Asset with Dynamic Classification
    public DbSet<PhysicalAsset> PhysicalAssets { get; set; } = null!;
    public DbSet<AssetClassificationRule> AssetClassificationRules { get; set; } = null!;
    public DbSet<RcaAccountCodeDefinition> RcaAccountCodes { get; set; } = null!;
    public DbSet<PPETypeAccountMapping> PPETypeAccountMappings { get; set; } = null!;

    // Configuration Tables for Database-Driven Behavior
    public DbSet<AssetConditionConfiguration> AssetConditionConfigurations { get; set; } = null!;
    public DbSet<PPETypeDefinition> PPETypeDefinitions { get; set; } = null!;
    public DbSet<UnitOfMeasure> UnitsOfMeasure { get; set; } = null!;
    public DbSet<PpeCategoryCode> PpeCategoryCodes { get; set; } = null!;
    public DbSet<PpeTypeCode> PpeTypeCodes { get; set; } = null!;
    public DbSet<NfaOfficeCode> NfaOfficeCodes { get; set; } = null!;
    public DbSet<PpeItemCode> PpeItemCodes { get; set; } = null!;
    public DbSet<PropertyCodeSequence> PropertyCodeSequences { get; set; } = null!;

    // Procurement Planning (PPMP)
    public DbSet<ProcurementPlanHeader> ProcurementPlans { get; set; } = null!;

    // Annual Procurement Planning (APP)
    public DbSet<AnnualProcurementPlanHeader> AnnualProcurementPlans { get; set; } = null!;

    // Procurement Projects
    public DbSet<ProcurementProject> ProcurementProjects { get; set; } = null!;

    // UI Workflow - Asset Requisition & Acceptance
    public DbSet<AssetRequisition> AssetRequisitions { get; set; } = null!;

    // UI Workflow - Depreciation & Accounting
    public DbSet<DepreciationSchedule> DepreciationSchedules { get; set; } = null!;
    public DbSet<JournalEntryVoucher> JournalEntryVouchers { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;

    // Supplies and Materials Issuance/Receiving Reports (NFA Philippines)
    public DbSet<SuppliesAndMaterialsIssuanceReport> SuppliesAndMaterialsIssuanceReports { get; set; } = null!;
    public DbSet<SuppliesAndMaterialsReceivingReport> SuppliesAndMaterialsReceivingReports { get; set; } = null!;
    public DbSet<SuppliesAndMaterialsReceivingLineItem> SuppliesAndMaterialsReceivingLineItems { get; set; } = null!;

    // PPE Issuance/Receiving Reports (NFA Philippines)
    public DbSet<PPEIR> PPEIRs { get; set; } = null!;
    public DbSet<PPEIRLineItem> PPEIRLineItems { get; set; } = null!;
    public DbSet<PPERR> PPERRs { get; set; } = null!;
    public DbSet<PPERRLineItem> PPERRLineItems { get; set; } = null!;

    // Property Accountability Receipt (PAR)
    public DbSet<PropertyAcknowledgementReceipt> PropertyAcknowledgementReceipts { get; set; } = null!;

    // Inventory Custodian Slip (ICS)
    public DbSet<InventoryCustodianSlip> InventoryCustodianSlips { get; set; } = null!;

    // Inventory Registry & Audit
    public DbSet<InventoryRegistry> InventoryRegistries { get; set; } = null!;
    public DbSet<InventoryTransactionLog> InventoryTransactionLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoriesDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Inventories);
    }
}


