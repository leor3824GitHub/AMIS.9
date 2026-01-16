using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class IssuanceConfiguration : IEntityTypeConfiguration<Issuance>
{
    public void Configure(EntityTypeBuilder<Issuance> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        
        // Existing properties
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.IssuanceDate).IsRequired();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.IsClosed).IsRequired();
        
        // New properties for acceptance workflow
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.CustodianId);
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.AcceptedOn);
        builder.Property(x => x.RejectionReason).HasMaxLength(500);

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

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Issuance)
            .HasForeignKey(x => x.IssuanceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.CustodianId);
        builder.HasIndex(x => x.EmployeeId);
    }
}
