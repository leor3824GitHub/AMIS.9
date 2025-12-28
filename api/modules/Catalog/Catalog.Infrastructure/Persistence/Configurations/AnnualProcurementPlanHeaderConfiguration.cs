using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class AnnualProcurementPlanHeaderConfiguration : IEntityTypeConfiguration<AnnualProcurementPlanHeader>
{
    public void Configure(EntityTypeBuilder<AnnualProcurementPlanHeader> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("AnnualProcurementPlans", SchemaNames.Procurement);

        builder.Property(x => x.ControlNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TotalBudget).HasPrecision(18, 2);
        builder.Property(x => x.RejectionReason).HasMaxLength(1000);

        // Unique per tenant
        builder.HasIndex("TenantId", nameof(AnnualProcurementPlanHeader.ControlNumber)).IsUnique();

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(i => i.PlanHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
