using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class PpeTypeCodeConfiguration : IEntityTypeConfiguration<PpeTypeCode>
{
    public void Configure(EntityTypeBuilder<PpeTypeCode> builder)
    {
        builder.ToTable("PPETypeCodes", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.CategoryId)
            .IsRequired();

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.SortOrder)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.COAReference)
            .HasMaxLength(100);

        builder.HasIndex(e => new { e.CategoryId, e.Code })
            .IsUnique();
    }
}
