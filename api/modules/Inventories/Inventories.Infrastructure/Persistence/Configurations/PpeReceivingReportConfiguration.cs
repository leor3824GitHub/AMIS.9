using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PpeReceivingReportConfiguration : IEntityTypeConfiguration<PpeReceivingReport>
{
    public void Configure(EntityTypeBuilder<PpeReceivingReport> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PpeReceivingReport));

        builder.Property(x => x.ReportNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ReportNumber).IsUnique();

        builder.Property(x => x.Location).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Notes).HasMaxLength(1000);

        // Source Information
        builder.OwnsOne(x => x.Source, source =>
        {
            source.Property(x => x.Name).HasMaxLength(200).HasColumnName("SourceName");
            source.Property(x => x.Address).HasMaxLength(500).HasColumnName("SourceAddress");
            source.Property(x => x.ReceiptDate).HasColumnName("SourceReceiptDate");
        });

        // Receipt Type
        builder.Property(x => x.ReceiptType).IsRequired().HasConversion(
            v => v.Value,
            v => PpeReceiptType.FromString(v));

        // Line Items
        builder.OwnsMany(x => x.LineItems, lineItems =>
        {
            lineItems.ToJson();
            lineItems.Property(li => li.PropertyCode).HasMaxLength(50);
            lineItems.Property(li => li.Description).HasMaxLength(500);
            lineItems.Property(li => li.Quantity).HasPrecision(18, 4);
            lineItems.Property(li => li.Unit).HasMaxLength(20);
            lineItems.Property(li => li.UnitCost).HasPrecision(18, 2);
            lineItems.Property(li => li.Location).HasMaxLength(200);
        });

        // Audit fields
        builder.Property(x => x.Created).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.CreatedBy).HasMaxLength(200);
        builder.Property(x => x.LastModified).HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.LastModifiedBy).HasMaxLength(200);
    }
}

