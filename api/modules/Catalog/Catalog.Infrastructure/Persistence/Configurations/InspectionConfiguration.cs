using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
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

            // Map optional InspectionRequestId FK (nullable for backward compatibility)
            builder.Property(x => x.InspectionRequestId).IsRequired(false);
            builder.HasOne(i => i.InspectionRequest)
                .WithMany() // Assuming InspectionRequest does NOT have a collection of Inspections
                .HasForeignKey(i => i.InspectionRequestId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete of inspection request

            // Property configurations
            builder.Property(x => x.InspectedOn)
                .IsRequired(false);

            builder.Property(x => x.Remarks)
                .HasMaxLength(200)
                .IsRequired(false);

            // Store enum as string in DB
            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();

            // Approved is required boolean
            builder.Property(x => x.Approved)
                .IsRequired();
        }
    }
}
