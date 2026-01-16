using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class JournalEntryConfiguration : IEntityTypeConfiguration<JournalEntry>
{
    public void Configure(EntityTypeBuilder<JournalEntry> builder)
    {
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.JournalEntryVoucherId).IsRequired();
        builder.Property(x => x.LineNumber).IsRequired();
        builder.Property(x => x.AccountCode)
            .IsRequired()
            .HasMaxLength(20);
        builder.Property(x => x.AccountName).HasMaxLength(150);
        builder.Property(x => x.DebitAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.CreditAmount)
            .IsRequired()
            .HasPrecision(18, 2);
        builder.Property(x => x.Description).HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.JournalEntryVoucher)
            .WithMany(x => x.Entries)
            .HasForeignKey(x => x.JournalEntryVoucherId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => new { x.JournalEntryVoucherId, x.LineNumber }).IsUnique();
        builder.HasIndex(x => x.AccountCode);
    }
}
