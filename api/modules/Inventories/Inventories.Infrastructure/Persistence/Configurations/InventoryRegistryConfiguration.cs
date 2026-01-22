using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class InventoryRegistryConfiguration : IEntityTypeConfiguration<InventoryRegistry>
{
    public void Configure(EntityTypeBuilder<InventoryRegistry> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(InventoryRegistry));

        builder.Property(x => x.PropertyCode).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.PropertyCode).IsUnique();

        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Location).HasMaxLength(255);
        builder.Property(x => x.Status).HasConversion<int>();

        builder.Property(x => x.ReceivedDate).IsRequired();
        builder.Property(x => x.IssuedDate);
        builder.Property(x => x.LastTransactionDate).IsRequired();
        builder.Property(x => x.LastTransactionType).HasMaxLength(20);
        builder.Property(x => x.LastTransactionReference).HasMaxLength(50);

        builder.Property(x => x.Quantity).IsRequired();

        // Auditable base properties
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.Created).IsRequired();
        builder.Property(x => x.LastModifiedBy);
        builder.Property(x => x.LastModified);
        builder.Property(x => x.DeletedBy);
        builder.Property(x => x.Deleted);

        // Seed test data
        SeedTestData(builder);
    }

    private static void SeedTestData(EntityTypeBuilder<InventoryRegistry> builder)
    {
        var now = DateTimeOffset.UtcNow;
        var seedData = new[]
        {
            CreateSeedItem(Guid.NewGuid(), "234", "Desktop Computer", 10, "IT Office - Room 101", "PPERR-001", now),
            CreateSeedItem(Guid.NewGuid(), "235", "Laptop Computer", 5, "IT Office - Room 102", "PPERR-001", now),
            CreateSeedItem(Guid.NewGuid(), "236", "Printer", 3, "IT Office - Room 103", "PPERR-001", now),
            CreateSeedItem(Guid.NewGuid(), "237", "Office Chair", 20, "Main Office", "PPERR-002", now),
            CreateSeedItem(Guid.NewGuid(), "238", "Desk Lamp", 15, "Main Office", "PPERR-002", now),
        };

        builder.HasData(seedData);
    }

    private static object CreateSeedItem(Guid id, string propertyCode, string description, int quantity, string location, string reportNumber, DateTimeOffset timestamp)
    {
        return new
        {
            Id = id,
            PropertyCode = propertyCode,
            Description = description,
            Quantity = quantity,
            Location = location,
            Status = InventoryItemStatus.InStock,
            ReceivedDate = DateTime.SpecifyKind(timestamp.DateTime, DateTimeKind.Utc),
            IssuedDate = (DateTime?)null,
            LastTransactionDate = DateTime.SpecifyKind(timestamp.DateTime, DateTimeKind.Utc),
            LastTransactionType = "PPERR",
            LastTransactionReference = reportNumber,
            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Created = timestamp,
            LastModifiedBy = (Guid?)null,
            LastModified = timestamp,
            DeletedBy = (Guid?)null,
            Deleted = (DateTimeOffset?)null
        };
    }
}
