using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class InventoryCustodianSlipConfiguration : IEntityTypeConfiguration<InventoryCustodianSlip>
{
    public void Configure(EntityTypeBuilder<InventoryCustodianSlip> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(InventoryCustodianSlip));

        builder.Property(x => x.ICSNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ICSNumber).IsUnique();

        builder.Property(x => x.Status).IsRequired();

        // Custodian/Employee Information
        builder.Property(x => x.EmployeeId).IsRequired();
        builder.HasIndex(x => x.EmployeeId);
        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

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
            lineItems.Property(li => li.Quantity).IsRequired();
            lineItems.Property(li => li.DateAcquired).IsRequired();
            lineItems.Property(li => li.UnitCost).IsRequired().HasPrecision(18, 2);
            lineItems.Property(li => li.Condition).HasMaxLength(100);
            lineItems.Property(li => li.Remarks).HasMaxLength(500);
        });

        // Return Information
        builder.Property(x => x.ReturnDate);
        builder.Property(x => x.ReturnRemarks).HasMaxLength(1000);
        builder.Property(x => x.ReceivedByEmployeeId);
        builder.HasOne(x => x.ReceivedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.ReceivedByEmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

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
