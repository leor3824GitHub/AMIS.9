using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class AcceptanceConfiguration : IEntityTypeConfiguration<Acceptance>
    {
        public void Configure(EntityTypeBuilder<Acceptance> builder)
        {
            
            builder.IsMultiTenant();   // MultiTenant support for Acceptance entity            
            builder.HasKey(x => x.Id);  // Primary key            
            builder.Property(x => x.AcceptanceDate)  // Property configurations
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(200)
                .IsRequired(false); // Optional, adjust based on your design

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
        }
    }
}
