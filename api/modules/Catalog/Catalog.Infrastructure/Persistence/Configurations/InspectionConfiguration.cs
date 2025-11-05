using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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

            // Link to InspectionRequest (required relationship)
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

            // Status stored as string; coalesce NULL provider values to a safe default to avoid materialization errors
            builder.Property(x => x.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => string.IsNullOrEmpty(v) ? InspectionStatus.InProgress : Enum.Parse<InspectionStatus>(v))
                .HasMaxLength(32)
                .IsRequired();

            // Approved may be NULL in legacy data; map NULL to false on read
            builder.Property(x => x.Approved)
                .HasConversion(new ValueConverter<bool, bool?>(v => v, v => v ?? false))
                .IsRequired();

        }
    }
}
