using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for PPETypeDefinition
/// </summary>
public class PPETypeDefinitionConfiguration : IEntityTypeConfiguration<PPETypeDefinition>
{
    public void Configure(EntityTypeBuilder<PPETypeDefinition> builder)
    {
        builder.ToTable("PPETypeDefinitions", SchemaNames.Catalog);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.RCAAccountCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DepreciationAccountCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.DefaultDepreciationRate)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(e => e.DefaultUsefulLifeYears)
            .IsRequired();

        builder.Property(e => e.SortOrder)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.COAReference)
            .HasMaxLength(100);

        builder.Property(e => e.Category)
            .HasMaxLength(100);

        builder.Property(e => e.IconName)
            .HasMaxLength(50);

        // Seed default data
        var defaults = PPETypeDefinition.SeedDefaults();
        builder.HasData(defaults);
    }
}
