using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Constants;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class ProcurementProjectConfiguration : IEntityTypeConfiguration<ProcurementProject>
{
    public void Configure(EntityTypeBuilder<ProcurementProject> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);

        builder.ToTable("ProcurementProjects", SchemaNames.Procurement);

        builder.Property(x => x.PapCode).HasMaxLength(50);
        builder.Property(x => x.ProjectTitle).HasMaxLength(500).IsRequired();
        builder.Property(x => x.PmoEndUser).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FundSource).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Remarks).HasMaxLength(1000);

        // Owned entity: Schedule
        builder.OwnsOne(x => x.Schedule, schedule =>
        {
            schedule.ToTable("ProcurementSchedules", SchemaNames.Procurement);
            schedule.WithOwner().HasForeignKey(s => s.ProjectId);
            schedule.HasKey(s => s.Id);
        });

        // Owned entity: Budget
        builder.OwnsOne(x => x.Budget, budget =>
        {
            budget.ToTable("ProjectBudgets", SchemaNames.Procurement);
            budget.WithOwner().HasForeignKey(b => b.ProjectId);
            budget.HasKey(b => b.Id);
            budget.Property(b => b.TotalAmount).HasPrecision(18, 2);
            budget.Property(b => b.MooeAmount).HasPrecision(18, 2);
            budget.Property(b => b.CoAmount).HasPrecision(18, 2);
        });
    }
}
