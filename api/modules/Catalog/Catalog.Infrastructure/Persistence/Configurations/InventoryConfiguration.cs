using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Location).HasMaxLength(100);
        builder.Property(x => x.ProductId).IsRequired();
    }
}
