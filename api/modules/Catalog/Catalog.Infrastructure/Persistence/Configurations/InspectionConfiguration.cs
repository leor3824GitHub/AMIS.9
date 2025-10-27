using AMIS.WebApi.Catalog.Domain;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations
{
    internal sealed class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
    {
        public void Configure(EntityTypeBuilder<Inspection> builder)
        {
            // Enable multi-tenancy
            builder.IsMultiTenant(); // Adds TenantId column and filter automatically

            // Primary key
            builder.HasKey(x => x.Id);

            // Foreign key relationships
            builder.HasOne(i => i.Employee)
                .WithMany() // Assuming Employee does NOT have a collection of Inspections
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if Inspector is deleted

            builder.HasOne(i => i.Purchase)
                .WithMany(p => p.Inspections)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade); // Usually OK to cascade if Purchase is deleted

            // Property configurations
            builder.Property(x => x.InspectedOn)
                .IsRequired();

            builder.Property(x => x.Remarks)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

        }
    }
}
