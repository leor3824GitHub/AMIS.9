using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core configuration for JournalEntryVoucher (JEV)
/// COA Circular 2022-002 compliance
/// </summary>
internal sealed class JournalEntryVoucherConfiguration : IEntityTypeConfiguration<JournalEntryVoucher>
{
    public void Configure(EntityTypeBuilder<JournalEntryVoucher> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("JournalEntryVouchers", global::Shared.Constants.SchemaNames.Catalog);

        builder.Property(x => x.JEVNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.TransactionDate)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.SourceDocumentType)
            .HasMaxLength(50);

        builder.Property(x => x.SourceDocumentNumber)
            .HasMaxLength(100);

        builder.Property(x => x.TotalDebit)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.TotalCredit)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.IsPosted)
            .HasDefaultValue(false);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(x => x.JEVNumber)
            .IsUnique();

        builder.HasIndex(x => x.TransactionDate);

        builder.HasIndex(x => x.SourceTransactionId);

        builder.HasIndex(x => x.IsPosted);

        // Relationships
        builder.HasMany(x => x.Lines)
            .WithOne(x => x.JournalEntryVoucher)
            .HasForeignKey(x => x.JournalEntryVoucherId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

/// <summary>
/// EF Core configuration for JournalEntryLine
/// </summary>
internal sealed class JournalEntryLineConfiguration : IEntityTypeConfiguration<JournalEntryLine>
{
    public void Configure(EntityTypeBuilder<JournalEntryLine> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("JournalEntryLines", global::Shared.Constants.SchemaNames.Catalog);

        builder.Property(x => x.AccountCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.AccountTitle)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.DebitAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.CreditAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Particulars)
            .HasMaxLength(500);

        // Indexes
        builder.HasIndex(x => x.JournalEntryVoucherId);

        builder.HasIndex(x => x.AccountCode);
    }
}
