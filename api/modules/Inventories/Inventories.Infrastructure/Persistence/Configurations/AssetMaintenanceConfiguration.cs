using Finbuckle.MultiTenant;
using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for AssetMaintenance entity
/// Defines table structure, relationships, and database constraints
/// </summary>
internal sealed class AssetMaintenanceConfiguration : IEntityTypeConfiguration<AssetMaintenance>
{
    public void Configure(EntityTypeBuilder<AssetMaintenance> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.ToTable("AssetMaintenances");

        // Core Properties
        builder.Property(x => x.PhysicalAssetId).IsRequired();
        builder.Property(x => x.AssetPropertyCode)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.AssetDescription)
            .IsRequired()
            .HasMaxLength(500);

        // Maintenance Details
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);
        builder.Property(x => x.ScheduledDate).IsRequired();

        // Status Tracking
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.StartedOn);
        builder.Property(x => x.CompletedOn);
        builder.Property(x => x.CompletionNotes).HasMaxLength(1000);
        builder.Property(x => x.FindingsNotes).HasMaxLength(1000);

        // Personnel & Responsibility
        builder.Property(x => x.ScheduledBy).IsRequired();
        builder.Property(x => x.PerformedBy);
        builder.Property(x => x.ApprovedBy);

        // Cost Tracking
        builder.Property(x => x.EstimatedCost).HasPrecision(18, 2);
        builder.Property(x => x.ActualCost).HasPrecision(18, 2);
        builder.Property(x => x.CostReference).HasMaxLength(100);

        // Cancellation Details
        builder.Property(x => x.CancelledOn);
        builder.Property(x => x.CancelledBy);
        builder.Property(x => x.CancellationReason).HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.Asset)
            .WithMany(x => x.MaintenanceHistory)
            .HasForeignKey(x => x.PhysicalAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ScheduledByEmployee)
            .WithMany()
            .HasForeignKey(x => x.ScheduledBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PerformedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.PerformedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.ApprovedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CancelledByEmployee)
            .WithMany()
            .HasForeignKey(x => x.CancelledBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for common queries
        builder.HasIndex(x => x.PhysicalAssetId).HasDatabaseName("IX_AssetMaintenances_PhysicalAssetId");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_AssetMaintenances_Status");
        builder.HasIndex(x => x.ScheduledDate).HasDatabaseName("IX_AssetMaintenances_ScheduledDate");
        builder.HasIndex(x => x.Type).HasDatabaseName("IX_AssetMaintenances_Type");
        builder.HasIndex(x => x.AssetPropertyCode).HasDatabaseName("IX_AssetMaintenances_PropertyCode");
        builder.HasIndex(x => new { x.Status, x.ScheduledDate }).HasDatabaseName("IX_AssetMaintenances_StatusScheduled");
        builder.HasIndex(x => new { x.Status, x.PerformedBy }).HasDatabaseName("IX_AssetMaintenances_StatusTechnician");
    }
}
