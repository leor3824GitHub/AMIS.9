using Finbuckle.MultiTenant;
using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

internal sealed class AssetRequisitionConfiguration : IEntityTypeConfiguration<AssetRequisition>
{
    public void Configure(EntityTypeBuilder<AssetRequisition> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.IssuanceId).IsRequired();
        builder.Property(x => x.RequisitionDate).IsRequired();
        builder.Property(x => x.ResponseDate);
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();
        builder.Property(x => x.RejectionReason).HasMaxLength(500);
        builder.Property(x => x.ExpirationDate);

        // Owned Type - DigitalSignature
        builder.OwnsOne(x => x.AcceptanceSignature, nav =>
        {
            nav.Property(x => x.SignatureData).HasMaxLength(5000);
            nav.Property(x => x.SignedOn);
            nav.Property(x => x.SignedByEmployeeId);
            nav.Property(x => x.IpAddress).HasMaxLength(45);
            nav.Property(x => x.UserAgent).HasMaxLength(500);
            nav.Property(x => x.DeviceFingerprint).HasMaxLength(256);
        });

        // Relationships
        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Issuance)
            .WithMany()
            .HasForeignKey(x => x.IssuanceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.ExpirationDate);
        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => x.IssuanceId);
    }
}

