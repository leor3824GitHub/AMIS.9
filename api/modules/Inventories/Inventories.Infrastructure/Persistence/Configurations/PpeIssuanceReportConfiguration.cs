using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PPEIRConfiguration : IEntityTypeConfiguration<PPEIR>
{
    public void Configure(EntityTypeBuilder<PPEIR> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PPEIR));

        builder.Property(x => x.IRNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.IRNumber).IsUnique();

        builder.Property(x => x.IssuedTo).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(1000);
        builder.Property(x => x.Status).IsRequired();

        // Issuance Type
        builder.Property(x => x.Type).IsRequired().HasConversion(
            v => v.Value,
            v => PpeIssueType.FromString(v));

        // Line Items - stored as JSON for simplicity
        builder.HasMany<PPEIRLineItem>()
            .WithOne()
            .HasForeignKey("PPEIRId");

        // Auditable base properties
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);
    }
}

/// <summary>
/// Configuration for PPEIR Line Items
/// </summary>
public sealed class PPEIRLineItemConfiguration : IEntityTypeConfiguration<PPEIRLineItem>
{
    public void Configure(EntityTypeBuilder<PPEIRLineItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PPEIRLineItem));

        builder.Property(x => x.PropertyCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IRNumber).IsRequired().HasMaxLength(50);

        // Foreign key
        builder.Property<Guid>("PPEIRId").IsRequired();
        builder.HasOne<PPEIR>()
            .WithMany()
            .HasForeignKey("PPEIRId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
