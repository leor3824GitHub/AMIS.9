using Finbuckle.MultiTenant;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for AssetDisposal aggregate
/// Defines table structure, relationships, and database constraints
/// </summary>
internal sealed class AssetDisposalConfiguration : IEntityTypeConfiguration<AssetDisposal>
{
    public void Configure(EntityTypeBuilder<AssetDisposal> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.ToTable("AssetDisposals");

        // Core Properties
        builder.Property(x => x.PhysicalAssetId).IsRequired();
        builder.Property(x => x.AssetPropertyCode)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.AssetDescription)
            .IsRequired()
            .HasMaxLength(500);

        // Request Details
        builder.Property(x => x.RequestedBy).IsRequired();
        builder.Property(x => x.RequestDate).IsRequired();
        builder.Property(x => x.DisposalMethod)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.Value,  // Convert to database string
                v => DisposalMethod.Parse(v));  // Convert back from database string
        builder.Property(x => x.JustificationReason).HasMaxLength(1000);
        builder.Property(x => x.AssetConditionAtDisposal)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.Value,  // Convert to database string
                v => AssetCondition.Parse(v));  // Convert back from database string

        // Approval Workflow
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();
        builder.Property(x => x.ApprovedBy);
        builder.Property(x => x.ApprovedOn);
        builder.Property(x => x.ApprovalNotes).HasMaxLength(500);

        // Completion Details
        builder.Property(x => x.CompletedOn);
        builder.Property(x => x.CompletedBy);
        builder.Property(x => x.SalvageValue).HasPrecision(18, 2);
        builder.Property(x => x.GainOrLoss).HasPrecision(18, 2);
        builder.Property(x => x.DisposalReferenceNumber).HasMaxLength(100);

        // Cancellation Details
        builder.Property(x => x.CancelledOn);
        builder.Property(x => x.CancelledBy);
        builder.Property(x => x.CancellationReason).HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.Asset)
            .WithMany(x => x.Disposals)
            .HasForeignKey(x => x.PhysicalAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.RequestedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.RequestedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CompletedByEmployee)
            .WithMany()
            .HasForeignKey(x => x.CompletedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.CancelledByEmployee)
            .WithMany()
            .HasForeignKey(x => x.CancelledBy)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes for common queries
        builder.HasIndex(x => x.PhysicalAssetId).HasDatabaseName("IX_AssetDisposals_PhysicalAssetId");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_AssetDisposals_Status");
        builder.HasIndex(x => x.RequestDate).HasDatabaseName("IX_AssetDisposals_RequestDate");
        builder.HasIndex(x => x.AssetPropertyCode).HasDatabaseName("IX_AssetDisposals_PropertyCode");
        builder.HasIndex(x => new { x.Status, x.ApprovedBy }).HasDatabaseName("IX_AssetDisposals_StatusApprover");
    }
}
