using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PurchaseDate).IsRequired(false);
        builder.Property(x => x.SupplierId).IsRequired(false);

        // Store status as string to align with text column
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired(false);

        // New fields
        builder.Property(x => x.ReferenceNumber)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.Currency)
            .HasMaxLength(16)
            .IsRequired(false);

        // Aggregate relationship: Purchase (root) -> PurchaseItems (children)
        // Explicitly configure FK and delete behavior Restrict to prevent accidental cascade deletions.
        builder
            .HasMany(x => x.Items)
            .WithOne() // No back navigation declared on PurchaseItem
            .HasForeignKey(pi => pi.PurchaseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Helpful index for fetching items under a purchase
        builder.HasIndex(x => x.SupplierId);
    }
}
