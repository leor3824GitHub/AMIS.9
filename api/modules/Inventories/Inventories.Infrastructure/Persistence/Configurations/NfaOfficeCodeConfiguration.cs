using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class NfaOfficeCodeConfiguration : IEntityTypeConfiguration<NfaOfficeCode>
{
    public void Configure(EntityTypeBuilder<NfaOfficeCode> builder)
    {
        builder.ToTable("NfaOfficeCodes", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.OfficeName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.ParentOfficeCode)
            .HasMaxLength(10);

        builder.Property(e => e.SortOrder)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();
    }
}
