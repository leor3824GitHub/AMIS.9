using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class IssuanceConfiguration : IEntityTypeConfiguration<Issuance>
{
    public void Configure(EntityTypeBuilder<Issuance> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Product).IsRequired();
        builder.Property(x => x.EmployeeId).IsRequired();
    }
}
