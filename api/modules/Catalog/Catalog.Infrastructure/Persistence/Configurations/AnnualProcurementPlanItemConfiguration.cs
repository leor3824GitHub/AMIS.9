using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class AnnualProcurementPlanItemConfiguration : IEntityTypeConfiguration<AnnualProcurementPlanItem>
{
    public void Configure(EntityTypeBuilder<AnnualProcurementPlanItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("AnnualProcurementPlanItems", SchemaNames.Catalog);

        builder.Property(x => x.PlanHeaderId).IsRequired();

        builder.Property(x => x.DepartmentName).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
        builder.Property(x => x.UnitOfMeasure).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Mode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ScheduleMonth).HasMaxLength(20).IsRequired();
        builder.Property(x => x.FundingSource).HasMaxLength(200).IsRequired();
        builder.Property(x => x.UnitCost).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.EstimatedBudget).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.PapCode).HasMaxLength(50);
        builder.Property(x => x.Remarks).HasMaxLength(1000);

        builder.HasIndex(x => x.PlanHeaderId);
    }
}
