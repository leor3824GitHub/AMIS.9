using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class CanvassConfiguration : IEntityTypeConfiguration<Canvass>
{
    public void Configure(EntityTypeBuilder<Canvass> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PurchaseRequestId).IsRequired();
        builder.Property(x => x.SupplierId).IsRequired();
        builder.Property(x => x.ItemDescription).IsRequired().HasMaxLength(512);
        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.Unit).IsRequired().HasMaxLength(50);
        builder.Property(x => x.QuotedPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.Remarks).HasMaxLength(1024);
        builder.Property(x => x.ResponseDate).IsRequired();
        builder.Property(x => x.IsSelected).IsRequired().HasDefaultValue(false);

        // Foreign key relationships
        builder.HasOne(x => x.PurchaseRequest)
            .WithMany()
            .HasForeignKey(x => x.PurchaseRequestId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: one supplier can only quote once per purchase request
        builder.HasIndex(x => new { x.PurchaseRequestId, x.SupplierId })
            .IsUnique()
            .HasDatabaseName("IX_Canvass_PurchaseRequestId_SupplierId");

        // Index for querying by purchase request
        builder.HasIndex(x => x.PurchaseRequestId)
            .HasDatabaseName("IX_Canvass_PurchaseRequestId");

        // Index for querying by supplier
        builder.HasIndex(x => x.SupplierId)
            .HasDatabaseName("IX_Canvass_SupplierId");
    }
}
