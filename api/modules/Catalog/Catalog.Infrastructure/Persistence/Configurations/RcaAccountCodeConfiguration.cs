using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

public class RcaAccountCodeConfiguration : IEntityTypeConfiguration<RcaAccountCodeDefinition>
{
    public void Configure(EntityTypeBuilder<RcaAccountCodeDefinition> builder)
    {
        builder.ToTable("RcaAccountCodes");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Key)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(a => a.AccountCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(256);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(a => a.Key)
            .IsUnique();

        builder.HasQueryFilter(a => a.IsActive);
    }
}
