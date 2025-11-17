using Finbuckle.MultiTenant;
using AMIS.WebApi.Catalog.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Catalog.Infrastructure.Persistence.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.IsMultiTenant();
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Designation).HasMaxLength(100);

        // Configure ContactInformation as owned type
        builder.OwnsOne(x => x.ContactInfo, contactInfo =>
        {
            contactInfo.Property(c => c.Email)
                .HasMaxLength(256)
                .IsRequired();

            contactInfo.Property(c => c.PhoneNumber)
                .HasMaxLength(50)
                .IsRequired();
        });

        // Configure ResponsibilityCode as owned type
        builder.OwnsOne(x => x.ResponsibilityCode, respCode =>
        {
            respCode.Property(r => r.Value)
                .HasMaxLength(10)
                .HasColumnName("ResponsibilityCode")
                .IsRequired();
        });
    }
}
