using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class SuppliesAndMaterialsReceivingReportConfiguration
    : IEntityTypeConfiguration<SuppliesAndMaterialsReceivingReport>
{
    public void Configure(EntityTypeBuilder<SuppliesAndMaterialsReceivingReport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SmrrNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(x => x.Source, sourceBuilder =>
        {
            sourceBuilder.Property(s => s.Name)
                .HasMaxLength(200)
                .IsRequired();

            sourceBuilder.Property(s => s.Address)
                .HasMaxLength(500)
                .IsRequired();

            sourceBuilder.Property(s => s.ReceivingDate)
                .IsRequired();
        });

        builder.Property(x => x.TransactionType)
            .HasConversion(
                v => v.Value,
                v => ReceivingTransactionType.FromString(v))
            .IsRequired();

        builder.OwnsMany(x => x.LineItems, lineItemBuilder =>
        {
            lineItemBuilder.WithOwner().HasForeignKey("SuppliesAndMaterialsReceivingReportId");
            lineItemBuilder.HasKey("Id");

            lineItemBuilder.Property(li => li.Name)
                .HasMaxLength(200)
                .IsRequired();

            lineItemBuilder.Property(li => li.Description)
                .HasMaxLength(500)
                .IsRequired();

            lineItemBuilder.Property(li => li.Reference)
                .HasMaxLength(100);

            lineItemBuilder.Property(li => li.AcquisitionDate)
                .IsRequired();

            lineItemBuilder.Property(li => li.Quantity)
                .HasPrecision(18, 4)
                .IsRequired();

            lineItemBuilder.Property(li => li.Unit)
                .HasMaxLength(50)
                .IsRequired();

            lineItemBuilder.Property(li => li.UnitCost)
                .HasPrecision(18, 2)
                .IsRequired();

            lineItemBuilder.Property(li => li.Location)
                .HasMaxLength(200)
                .IsRequired();

            lineItemBuilder.Property(li => li.ClassCode)
                .HasMaxLength(2);

            lineItemBuilder.Property(li => li.CategoryCode)
                .HasMaxLength(2);

            lineItemBuilder.Property(li => li.ItemCode)
                .HasMaxLength(3);
        });

        builder.OwnsOne(x => x.Authentication, authBuilder =>
        {
            authBuilder.Property(a => a.ReceivedByName)
                .HasMaxLength(200)
                .IsRequired();

            authBuilder.Property(a => a.ReceivedDate)
                .IsRequired();

            authBuilder.Property(a => a.NotedByName)
                .HasMaxLength(200)
                .IsRequired();

            authBuilder.Property(a => a.NotedDate)
                .IsRequired();
        });

        builder.Property(x => x.DistributedToVoucher)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToPMSDS)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToAccounting)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToFile)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(x => x.SmrrNumber)
            .IsUnique();

        builder.ToTable("SuppliesAndMaterialsReceivingReports");
    }
}

