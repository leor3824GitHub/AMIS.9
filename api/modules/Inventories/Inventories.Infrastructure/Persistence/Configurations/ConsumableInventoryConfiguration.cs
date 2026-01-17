using Finbuckle.MultiTenant;
using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for ConsumableInventory
/// Maps to RCA 10501000 - Supplies and Materials Inventory
/// </summary>
internal sealed class ConsumableInventoryConfiguration : IEntityTypeConfiguration<ConsumableInventory>
{
    public void Configure(EntityTypeBuilder<ConsumableInventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("ConsumableInventories", global::Shared.Constants.SchemaNames.Inventories);

        builder.Property(x => x.StockNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.UnitCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitOfMeasure)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.WeightedAverageCost)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.ReorderLevel)
            .HasDefaultValue(0);

        // Indexes
        builder.HasIndex(x => x.StockNumber)
            .IsUnique();

        builder.HasIndex(x => x.ProductId);

        // Relationships
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

