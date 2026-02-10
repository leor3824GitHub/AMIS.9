using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PPERRConfiguration : IEntityTypeConfiguration<PPERR>
{
    public void Configure(EntityTypeBuilder<PPERR> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PPERR));

        // Primary identifier
        builder.Property(x => x.RRNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.RRNumber).IsUnique();

        // Report source information
        builder.Property(x => x.ReceivedFrom).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(500);

        // Receipt type
        builder.Property(x => x.Type).IsRequired().HasConversion(
            v => v.Value,
            v => PpeReceiptType.FromString(v));

        // Date and metadata
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.Status).IsRequired();

        // Line Items collection
        builder.HasMany(x => x.Items)
            .WithOne()
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}


