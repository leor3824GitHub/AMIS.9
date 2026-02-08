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

        // Acquisition details
        builder.Property(x => x.AcquisitionCost)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.AcquisitionDate).IsRequired();

        // Physical attributes
        builder.Property(x => x.SerialNumber).HasMaxLength(100);
        builder.Property(x => x.ModelNumber).HasMaxLength(100);
        builder.Property(x => x.Condition)
            .IsRequired()
            .HasMaxLength(50);

        // Quantity and UOM
        builder.Property(x => x.Quantity).IsRequired();

        // Lifecycle
        builder.Property(x => x.DisposalDate);
        builder.Property(x => x.DisposalReason).HasMaxLength(500);

        // Classification - computed property, not mapped to database
        builder.Ignore(x => x.CurrentClassification);

        // PPE-specific
        builder.Property(x => x.AccumulatedDepreciation)
            .IsRequired()
            .HasPrecision(18, 2);

        // QR Code & Identification (NEW)
        builder.Property(x => x.QRCodeData).HasMaxLength(5000);
        builder.Property(x => x.QRGeneratedDate);
        
        // CurrentCustodianId is now computed from CurrentAssignment.EmployeeId, not stored
        builder.Ignore(x => x.CurrentCustodianId);

        // Asset Hierarchy (Parent-Child)
        builder.Property(x => x.ParentAssetId);

        // Navigation
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Self-referential hierarchy: Parent Asset
        builder.HasOne(x => x.ParentAsset)
            .WithMany(x => x.SubAssets)
            .HasForeignKey(x => x.ParentAssetId)
            .OnDelete(DeleteBehavior.SetNull);

        // AssignmentHistory relationships are handled by their
        // PhysicalAssetId foreign keys and will be auto-discovered by EF Core

        // Indexes
        builder.HasIndex(x => x.PropertyCode).IsUnique();
        builder.HasIndex(x => x.DisposalDate);
    }
}

