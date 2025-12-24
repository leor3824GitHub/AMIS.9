using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for SemiExpendableInventory
/// Maps to RCA 10599020 - Semi-Expendable Property Inventory
/// </summary>
internal sealed class SemiExpendableInventoryConfiguration : IEntityTypeConfiguration<SemiExpendableInventory>
{
    public void Configure(EntityTypeBuilder<SemiExpendableInventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("SemiExpendableInventories", global::Shared.Constants.SchemaNames.Catalog);

        builder.Property(x => x.PropertyCode)
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

        builder.Property(x => x.EstimatedUsefulLifeMonths)
            .HasDefaultValue(36); // Default 3 years

        builder.Property(x => x.IsIssued)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(x => x.PropertyCode)
            .IsUnique();

        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x => x.CurrentCustodianId);

        builder.HasIndex(x => x.IsIssued);

        // Relationships
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CurrentCustodian)
            .WithMany()
            .HasForeignKey(x => x.CurrentCustodianId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
