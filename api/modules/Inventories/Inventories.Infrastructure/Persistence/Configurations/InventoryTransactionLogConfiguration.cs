using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class InventoryTransactionLogConfiguration : IEntityTypeConfiguration<InventoryTransactionLog>
{
    public void Configure(EntityTypeBuilder<InventoryTransactionLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(InventoryTransactionLog));

        builder.Property(x => x.PropertyCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.TransactionType).IsRequired().HasMaxLength(20);
        builder.Property(x => x.ReportNumber).IsRequired().HasMaxLength(50);

        builder.Property(x => x.QuantityChange).IsRequired();
        builder.Property(x => x.InventoryBefore).IsRequired();
        builder.Property(x => x.InventoryAfter).IsRequired();
        builder.Property(x => x.StatusBefore).HasConversion<int>();
        builder.Property(x => x.StatusAfter).HasConversion<int>();

        builder.Property(x => x.Success).IsRequired();
        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);
        builder.Property(x => x.InitiatedBy).HasMaxLength(256);
        builder.Property(x => x.TransactionDate).IsRequired();

        builder.HasIndex(x => new { x.PropertyCode, x.TransactionDate });

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}
