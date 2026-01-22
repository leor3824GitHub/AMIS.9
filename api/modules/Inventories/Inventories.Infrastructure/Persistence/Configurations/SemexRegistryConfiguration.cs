using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class SemexRegistryConfiguration : IEntityTypeConfiguration<SemexRegistry>
{
    public void Configure(EntityTypeBuilder<SemexRegistry> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(SemexRegistry));

        builder.Property(x => x.ItemCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.ItemCode).IsUnique();

        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Unit).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Location).HasMaxLength(255);
        builder.Property(x => x.Status).HasConversion<int>();
        builder.Property(x => x.UnitCost).HasPrecision(18, 2);

        builder.Property(x => x.ReceivedDate).IsRequired();
        builder.Property(x => x.IssuedDate);
        builder.Property(x => x.LastTransactionDate).IsRequired();
        builder.Property(x => x.LastTransactionType).HasMaxLength(20);
        builder.Property(x => x.LastTransactionReference).HasMaxLength(50);

        builder.Property(x => x.Quantity).IsRequired();

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}
