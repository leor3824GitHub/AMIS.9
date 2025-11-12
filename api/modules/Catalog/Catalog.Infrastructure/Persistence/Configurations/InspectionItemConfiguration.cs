using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class InspectionItemConfiguration : IEntityTypeConfiguration<InspectionItem>
    {
        public void Configure(EntityTypeBuilder<InspectionItem> builder)
        {
            // Enable multi-tenancy support via Finbuckle
            builder.IsMultiTenant();

            // Primary Key
            builder.HasKey(x => x.Id);

            // Required fields
            builder.Property(x => x.InspectionId)
                .IsRequired();

            builder.Property(x => x.PurchaseItemId)
                .IsRequired();

            builder.Property(x => x.QtyInspected)
                .IsRequired();

            builder.Property(x => x.QtyPassed)
                .IsRequired();

            builder.Property(x => x.QtyFailed)
                .IsRequired();

            // Optional enum status stored as string for compatibility
            builder.Property(x => x.InspectionItemStatus)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired(false);

            // Optional Remarks field
            builder.Property(x => x.Remarks)
                .HasMaxLength(500)
                .IsRequired(false); // optional

            // Enforce: a PurchaseItem can appear only once per Inspection (but can appear in different Inspections)
            builder.HasIndex(x => new { x.InspectionId, x.PurchaseItemId })
                .IsUnique();

            // Optional: keep a non-unique index on PurchaseItemId for query performance
            builder.HasIndex(x => x.PurchaseItemId);

            // Relationships
            builder.HasOne(x => x.Inspection)
                .WithMany(i => i.Items) // Maintain one-to-many
                .HasForeignKey(x => x.InspectionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting an Inspection if items exist
        }
    }
}
