using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PropertyAcknowledgementReceiptConfiguration : IEntityTypeConfiguration<PropertyAcknowledgementReceipt>
{
    public void Configure(EntityTypeBuilder<PropertyAcknowledgementReceipt> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PropertyAcknowledgementReceipt));

        builder.Property(x => x.PARNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.PARNumber).IsUnique();

        builder.Property(x => x.Status).IsRequired();

        // Custodian/Employee Information
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.HasIndex(x => x.EmployeeId);
        builder.Property(x => x.EmployeeName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Department).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Position).HasMaxLength(255);

        // Issuance Information
        builder.Property(x => x.IssuanceDate).IsRequired();
        builder.Property(x => x.IssuancePurpose).HasMaxLength(500);
        builder.Property(x => x.IssuanceLocation).HasMaxLength(200);

        // Line Items (stored as JSON)
        builder.OwnsMany(x => x.LineItems, lineItems =>
        {
            lineItems.ToJson();
            lineItems.Property(li => li.PropertyCode).IsRequired().HasMaxLength(50);
            lineItems.Property(li => li.Description).IsRequired().HasMaxLength(500);
            lineItems.Property(li => li.DateAcquired).IsRequired();
            lineItems.Property(li => li.AcquisitionCost).IsRequired().HasPrecision(18, 2);
            lineItems.Property(li => li.Condition).HasMaxLength(100);
            lineItems.Property(li => li.Remarks).HasMaxLength(500);
        });

        // Return Information
        builder.Property(x => x.ReturnDate);
        builder.Property(x => x.ReturnRemarks).HasMaxLength(1000);
        builder.Property(x => x.ReceivedByEmployeeId);
        builder.Property(x => x.ReceivedByEmployeeName).HasMaxLength(255);

        // Authentication/Signatures
        builder.Property(x => x.IssuedByName).HasMaxLength(255);
        builder.Property(x => x.IssuedByDate);
        builder.Property(x => x.ReceivedByName).HasMaxLength(255);
        builder.Property(x => x.ReceivedByDate);
        builder.Property(x => x.ApprovedByName).HasMaxLength(255);
        builder.Property(x => x.ApprovedByDate);

        // Additional metadata
        builder.Property(x => x.Notes).HasMaxLength(1000);

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}
