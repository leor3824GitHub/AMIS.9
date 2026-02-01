using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class PpeItemCodeConfiguration : IEntityTypeConfiguration<PpeItemCode>
{
    public void Configure(EntityTypeBuilder<PpeItemCode> builder)
    {
        builder.ToTable("PPEItemCodes", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.ClassCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.CategoryCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasIndex(e => new { e.ClassCode, e.CategoryCode, e.Code })
            .IsUnique();

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
    }
}
