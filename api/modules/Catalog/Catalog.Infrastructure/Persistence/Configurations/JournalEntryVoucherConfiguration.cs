using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class JournalEntryVoucherConfiguration : IEntityTypeConfiguration<JournalEntryVoucher>
{
    public void Configure(EntityTypeBuilder<JournalEntryVoucher> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.VoucherNumber)
            .IsRequired()
            .HasMaxLength(25);
        builder.Property(x => x.VoucherDate).IsRequired();
        builder.Property(x => x.Month).IsRequired();
        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.DepreciationMethod)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.TotalDebitAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.TotalCreditAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.Status).IsRequired().HasConversion<int>();
        builder.Property(x => x.PostedDate);
        builder.Property(x => x.ExportedDate);
        builder.Property(x => x.ExportFormat).HasMaxLength(20);
        builder.Property(x => x.ExportFileName).HasMaxLength(256);
        builder.Property(x => x.Remarks).HasMaxLength(500);

        // Navigation
        builder.HasMany(x => x.Entries)
            .WithOne(x => x.JournalEntryVoucher)
            .HasForeignKey(x => x.JournalEntryVoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.VoucherNumber).IsUnique();
        builder.HasIndex(x => new { x.Year, x.Month }).IsUnique();
        builder.HasIndex(x => x.Status);
    }
}
