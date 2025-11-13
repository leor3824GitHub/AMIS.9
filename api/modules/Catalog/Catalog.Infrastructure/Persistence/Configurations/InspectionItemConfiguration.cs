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

            // Enum status required (stored as int)
            builder.Property(x => x.InspectionItemStatus)
                .HasConversion<int>()
                .IsRequired();

            // Optional Remarks field
            builder.Property(x => x.Remarks)
                .HasMaxLength(500)
                .IsRequired(false); // optional

            // Enforce single-shot inspection per PurchaseItem across all inspections
            builder.HasIndex(x => x.PurchaseItemId)
                .IsUnique();

            // Relationships
            builder.HasOne(x => x.Inspection)
                .WithMany() // Assuming Inspection has no navigation collection
                .HasForeignKey(x => x.InspectionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
