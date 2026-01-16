using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

public class AssetAssignmentHistoryConfiguration : IEntityTypeConfiguration<AssetAssignmentHistory>
{
    public void Configure(EntityTypeBuilder<AssetAssignmentHistory> builder)
    {
        builder.ToTable("AssetAssignmentHistories");

        builder.HasOne(a => a.Asset)
            .WithMany(a => a.AssignmentHistory)
            .HasForeignKey(a => a.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TransferredToEmployee)
            .WithMany()
            .HasForeignKey(a => a.TransferredToEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.AcceptedByEmployee)
            .WithMany()
            .HasForeignKey(a => a.AcceptedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
