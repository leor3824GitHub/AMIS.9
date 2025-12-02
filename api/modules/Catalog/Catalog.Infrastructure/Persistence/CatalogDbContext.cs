using Finbuckle.MultiTenant.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.Framework.Infrastructure.Tenant;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContext : FshDbContext
{
    public CatalogDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<CatalogDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
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
    public DbSet<InspectionRequest> InspectionRequests { get; set; } = null!;
    public DbSet<PurchaseRequest> PurchaseRequests { get; set; } = null!;
    public DbSet<PurchaseRequestItem> PurchaseRequestItems { get; set; } = null!;
    public DbSet<Canvass> Canvasses { get; set; } = null!;

    // COA-Compliant Inventory Tables (Separate per Classification)
    public DbSet<ConsumableInventory> ConsumableInventories { get; set; } = null!;
    // Removed legacy separated tables in favor of unified PhysicalAsset
    // public DbSet<SemiExpendableInventory> SemiExpendableInventories { get; set; } = null!;
    // public DbSet<PropertyPlantEquipment> PropertyPlantEquipments { get; set; } = null!;
    // public DbSet<PPEAssignmentHistory> PPEAssignmentHistories { get; set; } = null!;

    // Unified Physical Asset with Dynamic Classification
    public DbSet<PhysicalAsset> PhysicalAssets { get; set; } = null!;
    public DbSet<AssetAssignmentHistory> AssetAssignmentHistories { get; set; } = null!;
    public DbSet<AssetReclassificationHistory> AssetReclassificationHistories { get; set; } = null!;
    public DbSet<AssetClassificationRule> AssetClassificationRules { get; set; } = null!;
    public DbSet<PPETypeAccountMapping> PPETypeAccountMappings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}
