using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class AcceptanceItemConfiguration : IEntityTypeConfiguration<AcceptanceItem>
    {
        public void Configure(EntityTypeBuilder<AcceptanceItem> builder)
        {
            // Enable multi-tenancy support via Finbuckle
            builder.IsMultiTenant();

            // Primary Key
            builder.HasKey(x => x.Id);

            // Required fields
            builder.Property(x => x.AcceptanceId)
                .IsRequired();

            builder.Property(x => x.PurchaseItemId)
                .IsRequired();

            builder.Property(x => x.QtyAccepted)
                .IsRequired();

            // DB-level guardrails
            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("CK_AcceptanceItems_QtyAccepted_NonNegative", "\"QtyAccepted\" >= 0");
            });

            // Prevent duplicate purchase item rows within the same acceptance
            builder.HasIndex(x => new { x.AcceptanceId, x.PurchaseItemId })
                .IsUnique();

            // Optional Remarks field
            builder.Property(x => x.Remarks)
                .HasMaxLength(500)
                .IsRequired(false); // optional

            // Relationships
            builder.HasOne(x => x.Acceptance)
                .WithMany(x => x.Items) // Assuming Acceptance has no navigation collection
                .HasForeignKey(x => x.AcceptanceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
