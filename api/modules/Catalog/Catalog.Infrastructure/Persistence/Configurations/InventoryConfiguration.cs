using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductId).IsRequired();

        // Ensure enums are stored as text to match existing PostgreSQL schema
        builder.Property(x => x.StockStatus)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.CostingMethod)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        // Optional location length constraint
        builder.Property(x => x.Location)
            .HasMaxLength(128);
    }
}
