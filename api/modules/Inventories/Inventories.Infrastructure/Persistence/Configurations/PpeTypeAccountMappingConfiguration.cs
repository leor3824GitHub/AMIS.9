using AMIS.WebApi.Inventories.Domain;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

internal sealed class PPETypeAccountMappingConfiguration : IEntityTypeConfiguration<PPETypeAccountMapping>
{
    public void Configure(EntityTypeBuilder<PPETypeAccountMapping> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.PPEType)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.RCAAccountCode)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Create unique index on PPEType for active records only
        builder.HasIndex(x => x.PPEType)
            .IsUnique()
            .HasFilter("\"IsActive\" = true AND \"Deleted\" IS NULL");
        
        // Audit fields
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.LastModified).IsRequired();
    }
}

