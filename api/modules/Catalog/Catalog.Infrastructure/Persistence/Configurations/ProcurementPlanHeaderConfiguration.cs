using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class ProcurementPlanHeaderConfiguration : IEntityTypeConfiguration<ProcurementPlanHeader>
{
    public void Configure(EntityTypeBuilder<ProcurementPlanHeader> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("ProcurementPlans", SchemaNames.Catalog);

        builder.Property(x => x.ControlNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DepartmentName).HasMaxLength(200);
        builder.Property(x => x.RejectionReason).HasMaxLength(1000);

        builder.Property(x => x.TotalBudget).HasPrecision(18, 2);

        // Unique per tenant
        builder.HasIndex("TenantId", nameof(ProcurementPlanHeader.ControlNumber)).IsUnique();

        // Use standard one-to-many relationship (not owned) to avoid EF Core owned/non-owned conflict
        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(i => i.PlanHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
