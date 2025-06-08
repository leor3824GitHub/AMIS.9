using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class InspectionRequestConfiguration : IEntityTypeConfiguration<InspectionRequest>
    {
        public void Configure(EntityTypeBuilder<InspectionRequest> builder)
        {
            
            builder.IsMultiTenant();   // MultiTenant support for Inspection entity            
            builder.HasKey(x => x.Id);  // Primary key            
            builder.Property(x => x.PurchaseId)  // Property configurations
                .IsRequired();
        }
    }
}
