using AMIS.WebApi.Inventories.Domain;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations
{
    internal sealed class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> builder)
        {
            // Enable multi-tenancy
            builder.IsMultiTenant(); // Adds TenantId column and filter automatically

            // Primary key
            builder.HasKey(x => x.Id);

            // Inspection type enum conversion
            builder.Property(x => x.Type)
                .HasConversion<int>()
                .IsRequired();

            // Foreign key relationships
            builder.HasOne(i => i.Employee)
                .WithMany() // Assuming Employee does NOT have a collection of Inspections
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if Inspector is deleted

            // Link to Purchase (for NewDelivery type)
            builder.HasOne(i => i.Purchase)
                .WithMany(p => p.Inspections)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Link to PhysicalAsset (for AssetReturn and Repair types)
            builder.HasOne(i => i.PhysicalAsset)
                .WithMany()
                .HasForeignKey(i => i.PhysicalAssetId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // One-to-many: Inspection has many InspectionItems
            builder.HasMany(i => i.Items)
                .WithOne(ii => ii.Inspection)
                .HasForeignKey(ii => ii.InspectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            builder.Property(x => x.InspectedOn)
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.IARDocumentPath)
                .HasMaxLength(500)
                .IsRequired(false);

            // Indexes for common queries
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.PurchaseId);
            builder.HasIndex(x => x.PhysicalAssetId);
            builder.HasIndex(x => x.EmployeeId);
        }
    }
}


