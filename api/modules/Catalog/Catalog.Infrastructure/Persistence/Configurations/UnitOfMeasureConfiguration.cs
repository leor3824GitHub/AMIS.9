using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for UnitOfMeasure
/// </summary>
public class UnitOfMeasureConfiguration : IEntityTypeConfiguration<UnitOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitOfMeasure> builder)
    {
        builder.ToTable("UnitsOfMeasure", SchemaNames.Catalog);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Abbreviation)
            .HasMaxLength(20);

        builder.Property(e => e.UnitType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.SortOrder)
            .IsRequired();

        builder.Property(e => e.IsDefault)
            .IsRequired();

        builder.Property(e => e.BaseUnitId);

        builder.Property(e => e.ConversionFactor)
            .HasPrecision(18, 6);

        // Self-referencing relationship for unit conversions
        builder.HasOne<UnitOfMeasure>()
            .WithMany()
            .HasForeignKey(e => e.BaseUnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed default data
        var defaults = UnitOfMeasure.SeedDefaults();
        builder.HasData(defaults);
    }
}
