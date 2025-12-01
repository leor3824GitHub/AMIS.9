using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(1000);

        builder.Property(x => x.PropertyClassification)
            .HasConversion<int>()
            .HasDefaultValue(PropertyClassification.Consumable)
            .HasComment("1=Consumable, 2=SemiExpendable, 3=PPE");

        builder.Property(x => x.EstimatedUsefulLife)
            .HasDefaultValue(12)
            .HasComment("Estimated useful life in months");

        builder.HasIndex(x => x.PropertyClassification);
    }
}
