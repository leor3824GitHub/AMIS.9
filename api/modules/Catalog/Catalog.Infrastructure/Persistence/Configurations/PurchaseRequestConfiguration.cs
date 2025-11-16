using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;
internal sealed class PurchaseRequestConfiguration : IEntityTypeConfiguration<PurchaseRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseRequest> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RequestDate).IsRequired();
        builder.Property(x => x.RequestedBy).IsRequired();
        builder.Property(x => x.Purpose).HasMaxLength(512).IsRequired();
        builder.Property(x => x.ApprovalRemarks).HasMaxLength(1024);
        builder.HasMany(x => x.Items).WithOne(i => i.PurchaseRequest).HasForeignKey(i => i.PurchaseRequestId);
    }
}
