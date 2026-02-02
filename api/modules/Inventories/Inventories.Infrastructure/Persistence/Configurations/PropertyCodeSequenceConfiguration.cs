using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class PropertyCodeSequenceConfiguration : IEntityTypeConfiguration<PropertyCodeSequence>
{
    public void Configure(EntityTypeBuilder<PropertyCodeSequence> builder)
    {
        builder.ToTable("PropertyCodeSequences", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.YearKey)
            .IsRequired();

        builder.Property(e => e.OfficeCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.ClassCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.CategoryCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.ItemCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(e => e.LastSequence)
            .IsRequired();

        builder.Property(e => e.ResetAnnually)
            .IsRequired();

        builder.HasIndex(e => new { e.YearKey, e.OfficeCode, e.ClassCode, e.CategoryCode, e.ItemCode })
            .IsUnique();
    }
}
