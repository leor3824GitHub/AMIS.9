using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public class PpeCategoryCodeConfiguration : IEntityTypeConfiguration<PpeCategoryCode>
{
    public void Configure(EntityTypeBuilder<PpeCategoryCode> builder)
    {
        builder.ToTable("PPECategoryCodes", SchemaNames.Inventories);

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(2);

        builder.HasIndex(e => e.Code)
            .IsUnique();

        builder.Property(e => e.AccountCode)
            .IsRequired()
            .HasMaxLength(20);

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

        builder.HasMany(e => e.TypeCodes)
            .WithOne(e => e.Category)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
