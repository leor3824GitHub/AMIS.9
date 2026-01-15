using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class DepreciationScheduleConfiguration : IEntityTypeConfiguration<DepreciationSchedule>
{
    public void Configure(EntityTypeBuilder<DepreciationSchedule> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.PhysicalAssetId).IsRequired();
        builder.Property(x => x.Month).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.MonthlyDepreciationAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.AccumulatedDepreciationAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();
        builder.Property(x => x.JournalEntryVoucherId);
        builder.Property(x => x.PostedDate);
        builder.Property(x => x.Remarks).HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.PhysicalAsset)
            .WithMany()
            .HasForeignKey(x => x.PhysicalAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.JournalEntryVoucher)
            .WithMany()
            .HasForeignKey(x => x.JournalEntryVoucherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(x => new { x.PhysicalAssetId, x.Year, x.Month }).IsUnique();
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.JournalEntryVoucherId);
    }
}
