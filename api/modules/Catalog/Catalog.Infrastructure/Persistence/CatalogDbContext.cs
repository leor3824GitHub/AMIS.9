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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);

        modelBuilder.Entity<Purchase>()
        .HasMany(p => p.Items)
        .WithOne()
        .HasForeignKey(i => i.PurchaseId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
