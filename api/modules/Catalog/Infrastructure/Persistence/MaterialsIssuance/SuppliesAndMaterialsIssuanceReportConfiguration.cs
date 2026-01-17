using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.Modules.Catalog.Infrastructure.Persistence.MaterialsIssuance;

public class SuppliesAndMaterialsIssuanceReportConfiguration
    : IEntityTypeConfiguration<SuppliesAndMaterialsIssuanceReport>
{
    public void Configure(EntityTypeBuilder<SuppliesAndMaterialsIssuanceReport> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.SmirNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.TransactionDate)
            .IsRequired();

        builder.OwnsOne(x => x.Recipient, recipientBuilder =>
        {
            recipientBuilder.Property(r => r.Name)
                .HasMaxLength(200)
                .IsRequired();

            recipientBuilder.Property(r => r.Address)
                .HasMaxLength(500)
                .IsRequired();

            recipientBuilder.Property(r => r.ContactNumber)
                .HasMaxLength(20);
        });

        builder.Property(x => x.IssuanceReason)
            .HasConversion(
                v => v.Value,
                v => IssuanceReason.FromString(v))
            .IsRequired();

        builder.OwnsMany(x => x.LineItems, lineItemBuilder =>
        {
            lineItemBuilder.WithOwner().HasForeignKey("SuppliesAndMaterialsIssuanceReportId");
            lineItemBuilder.HasKey("Id");

            lineItemBuilder.Property(li => li.Name)
                .HasMaxLength(200)
                .IsRequired();

            lineItemBuilder.Property(li => li.Description)
                .HasMaxLength(500)
                .IsRequired();

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
        });

        builder.OwnsOne(x => x.Authorization, authBuilder =>
        {
            authBuilder.Property(a => a.IssuingOfficerName)
                .HasMaxLength(200)
                .IsRequired();

            authBuilder.Property(a => a.IssuingOfficerSignature)
                .HasMaxLength(500)
                .IsRequired();

            authBuilder.Property(a => a.IssuingDate)
                .IsRequired();

            authBuilder.Property(a => a.ApprovingOfficerName)
                .HasMaxLength(200)
                .IsRequired();

            authBuilder.Property(a => a.ApprovingOfficerSignature)
                .HasMaxLength(500)
                .IsRequired();

            authBuilder.Property(a => a.ApprovingDate)
                .IsRequired();

            authBuilder.Property(a => a.RecipientName)
                .HasMaxLength(200)
                .IsRequired();

            authBuilder.Property(a => a.RecipientSignature)
                .HasMaxLength(500)
                .IsRequired();

            authBuilder.Property(a => a.ReceiptDate)
                .IsRequired();

            authBuilder.Property(a => a.DriverName)
                .HasMaxLength(200);

            authBuilder.Property(a => a.DriverSignature)
                .HasMaxLength(500);

            authBuilder.Property(a => a.BillOfLadingNumber)
                .HasMaxLength(100);
        });

        builder.Property(x => x.DistributedToRecipient)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToPMSDS)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToAccounting)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToAccountingAdvice)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.DistributedToFile)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.UpdatedBy)
            .HasMaxLength(100);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Add an index for efficient queries
        builder.HasIndex(x => x.SmirNumber)
            .IsUnique();

        builder.HasIndex(x => new { x.IsDeleted, x.CreatedAt });
    }
}
