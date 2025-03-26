using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class IssuanceItemConfiguration : IEntityTypeConfiguration<IssuanceItem>
{
    public void Configure(EntityTypeBuilder<IssuanceItem> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.IssuanceId).IsRequired();
    }
}
