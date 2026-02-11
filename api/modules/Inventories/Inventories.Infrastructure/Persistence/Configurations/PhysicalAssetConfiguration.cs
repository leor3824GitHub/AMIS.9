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

        // Optimistic concurrency control using Version instead of LastModified
        // Version is incremented on every state-changing operation
        builder.Property(x => x.Version)
            .IsRequired()
            .HasDefaultValue(0)
            .IsConcurrencyToken();

        // CurrentCustodianId is now computed from CurrentAssignment.EmployeeId, not stored
        builder.Ignore(x => x.CurrentCustodianId);

        // RCAAccountCode is computed from CurrentClassification and DefaultPolicy, not stored
        builder.Ignore(x => x.RCAAccountCode);

        // ImagePaths collection persisted as JSON (text type, not array)
        builder.Property(x => x.ImagePaths)
            .HasColumnType("text")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>());

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

        // AssignmentHistory navigation property (OneToMany)
        builder.HasMany(x => x.AssignmentHistory)
            .WithOne(x => x.Asset)
            .HasForeignKey(x => x.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Maintenance history navigation property (OneToMany)
        builder.HasMany(x => x.MaintenanceHistory)
            .WithOne(x => x.Asset)
            .HasForeignKey(x => x.PhysicalAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        // Disposals navigation property (OneToMany)
        builder.HasMany(x => x.Disposals)
            .WithOne(x => x.Asset)
            .HasForeignKey(x => x.PhysicalAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.PropertyCode).IsUnique();
        builder.HasIndex(x => x.DisposalDate);
    }
}

