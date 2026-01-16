using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity configuration for AssetConditionConfiguration
/// </summary>
public class AssetConditionConfigurationConfiguration : IEntityTypeConfiguration<AssetConditionConfiguration>
{
    public void Configure(EntityTypeBuilder<AssetConditionConfiguration> builder)
    {
        builder.ToTable("AssetConditionConfigurations", SchemaNames.Catalog);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.DisplayName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.ColorCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.SortOrder)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.AllowsForUse)
            .IsRequired();

        builder.Property(e => e.RequiresRepair)
            .IsRequired();

        builder.Property(e => e.RequiresDisposal)
            .IsRequired();

        // Seed default data
        var defaults = AssetConditionConfiguration.SeedDefaults();
        builder.HasData(defaults);
    }
}
