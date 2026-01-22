using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class SemexTransactionLogConfiguration : IEntityTypeConfiguration<SemexTransactionLog>
{
    public void Configure(EntityTypeBuilder<SemexTransactionLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(SemexTransactionLog));

        builder.Property(x => x.ItemCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ItemCode);

        builder.Property(x => x.TransactionType).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.TransactionType);

        builder.Property(x => x.ReportNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ReportNumber);

        builder.Property(x => x.StatusBefore).HasConversion<int>();
        builder.Property(x => x.StatusAfter).HasConversion<int>();

        builder.Property(x => x.Success).IsRequired();
        builder.Property(x => x.ErrorMessage).HasMaxLength(500);
        builder.Property(x => x.InitiatedBy).HasMaxLength(255);
        builder.Property(x => x.TransactionDate).IsRequired();
        builder.HasIndex(x => x.TransactionDate);

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}
