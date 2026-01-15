using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class AcceptanceConfiguration : IEntityTypeConfiguration<Acceptance>
    {
        public void Configure(EntityTypeBuilder<Acceptance> builder)
        {
            builder.IsMultiTenant();
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.PurchaseId).IsRequired();
            builder.Property(x => x.SupplyOfficerId).IsRequired();
            builder.Property(x => x.InspectionId);
            builder.Property(x => x.GoodsReceiptId);
            builder.Property(x => x.AcceptanceDate).IsRequired();
            builder.Property(x => x.Remarks).HasMaxLength(500);
            builder.Property(x => x.IsPosted).IsRequired();
            builder.Property(x => x.PostedOn);
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            // Relationships
            builder.HasOne(x => x.Purchase)
                .WithMany()
                .HasForeignKey(x => x.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SupplyOfficer)
                .WithMany()
                .HasForeignKey(x => x.SupplyOfficerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Inspection)
                .WithMany()
                .HasForeignKey(x => x.InspectionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.GoodsReceipt)
                .WithMany()
                .HasForeignKey(x => x.GoodsReceiptId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Acceptance)
                .HasForeignKey(x => x.AcceptanceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.PurchaseId);
            builder.HasIndex(x => x.SupplyOfficerId);
            builder.HasIndex(x => x.IsPosted);
        }
    }
}
