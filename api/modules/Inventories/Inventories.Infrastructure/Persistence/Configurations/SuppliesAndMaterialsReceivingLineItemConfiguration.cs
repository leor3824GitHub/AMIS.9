using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class SuppliesAndMaterialsReceivingLineItemConfiguration : IEntityTypeConfiguration<SuppliesAndMaterialsReceivingLineItem>
{
    public void Configure(EntityTypeBuilder<SuppliesAndMaterialsReceivingLineItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(SuppliesAndMaterialsReceivingLineItem));

        // Required properties
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.AcquisitionDate).IsRequired();
        builder.Property(x => x.Quantity).HasPrecision(18, 4);
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.UnitCost).HasPrecision(18, 2);
        builder.Property(x => x.Location).HasMaxLength(200);
        builder.Property(x => x.Reference).HasMaxLength(100);
        builder.Property(x => x.ClassCode).HasMaxLength(2);
        builder.Property(x => x.CategoryCode).HasMaxLength(2);
        builder.Property(x => x.ItemCode).HasMaxLength(3);
    }
}
