using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> builder)
        {
            
            builder.IsMultiTenant();   // MultiTenant support for Inspection entity            
            builder.HasKey(x => x.Id);  // Primary key            
            builder.Property(x => x.InspectionDate)  // Property configurations
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(200)
                .IsRequired(false); // Optional, adjust based on your design
        }
    }
}
