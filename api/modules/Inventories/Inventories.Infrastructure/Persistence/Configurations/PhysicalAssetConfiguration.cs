using Finbuckle.MultiTenant;
using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

internal sealed class PhysicalAssetConfiguration : IEntityTypeConfiguration<PhysicalAsset>
{
    public void Configure(EntityTypeBuilder<PhysicalAsset> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        // Core properties
        builder.Property(x => x.PropertyCode)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        // Acquisition details
        builder.Property(x => x.AcquisitionCost)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.AcquisitionDate).IsRequired();

        // Physical attributes
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.ModelNumber).HasMaxLength(100);
        builder.Property(x => x.Location).HasMaxLength(200);
        builder.Property(x => x.Condition)
            .IsRequired()
            .HasMaxLength(50);

        // Quantity and UOM
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.UnitOfMeasure)
            .IsRequired()
            .HasMaxLength(50);

        // Lifecycle
        builder.Property(x => x.EstimatedUsefulLife).IsRequired();
        builder.Property(x => x.DisposalDate);
        builder.Property(x => x.DisposalReason).HasMaxLength(500);

        // Classification
        builder.Property(x => x.CurrentClassification)
            .IsRequired()
            .HasConversion<string>();

        // PPE-specific
        builder.Property(x => x.PPEType).HasMaxLength(100);
        builder.Property(x => x.AccumulatedDepreciation)
            .IsRequired()
            .HasPrecision(18, 2);

        // QR Code & Identification (NEW)
        builder.Property(x => x.QRCodeData).HasMaxLength(5000);
        builder.Property(x => x.PropertyNumber).HasMaxLength(50);
        builder.Property(x => x.QRGeneratedDate);
        builder.Property(x => x.CurrentCustodianId);

        // Navigation
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // AssignmentHistory and ReclassificationHistory relationships are handled by their
        // PhysicalAssetId foreign keys and will be auto-discovered by EF Core

        // Indexes
        builder.HasIndex(x => x.PropertyCode).IsUnique();
        builder.HasIndex(x => x.PropertyNumber).IsUnique();
        builder.HasIndex(x => x.CurrentCustodianId);
        builder.HasIndex(x => x.CurrentClassification);
        builder.HasIndex(x => x.DisposalDate);
    }
}

