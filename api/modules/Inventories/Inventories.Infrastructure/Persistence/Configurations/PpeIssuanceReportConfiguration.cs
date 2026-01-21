using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PpeIssuanceReportConfiguration : IEntityTypeConfiguration<PpeIssuanceReport>
{
    public void Configure(EntityTypeBuilder<PpeIssuanceReport> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PpeIssuanceReport));

        builder.Property(x => x.ReportNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ReportNumber).IsUnique();

        builder.Property(x => x.IssuanceDate).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(1000);

        // Distribution Tracking
        builder.Property(x => x.DistributedToVoucher);
        builder.Property(x => x.DistributedToPMSDS);
        builder.Property(x => x.DistributedToAccounting);
        builder.Property(x => x.DistributedToFile);

        // Recipient Information
        builder.OwnsOne(x => x.Recipient, recipient =>
        {
            recipient.Property(r => r.Name).IsRequired().HasMaxLength(255).HasColumnName("RecipientName");
            recipient.Property(r => r.Address).IsRequired().HasMaxLength(500).HasColumnName("RecipientAddress");
        });

        // Issuance Type
        builder.Property(x => x.IssuanceType).IsRequired().HasConversion(
            v => v.Value,
            v => PpeIssuanceType.FromString(v));

        // Line Items
        builder.OwnsMany(x => x.LineItems, lineItems =>
        {
            lineItems.ToJson();
            lineItems.Property(li => li.PropertyCode).HasMaxLength(50);
            lineItems.Property(li => li.SerialNumber).HasMaxLength(100);
            lineItems.Property(li => li.Specification).HasMaxLength(500);
            lineItems.Property(li => li.DateAcquired);
            lineItems.Property(li => li.AcquisitionCost).HasPrecision(18, 2);
            lineItems.Property(li => li.AccumulatedDepreciation).HasPrecision(18, 2);
            lineItems.Property(li => li.BookValue).HasPrecision(18, 2);
            lineItems.Property(li => li.Location).HasMaxLength(200);
        });

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}
