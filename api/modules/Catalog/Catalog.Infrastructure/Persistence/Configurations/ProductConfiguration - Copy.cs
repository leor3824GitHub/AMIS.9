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
            // MultiTenant support for Inspection entity
            builder.IsMultiTenant();

            // Primary key
            builder.HasKey(x => x.Id);

            // Property configurations
            builder.Property(x => x.InspectionDate)
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(200)
                .IsRequired(false); // Optional, adjust based on your design
        }
    }
}
