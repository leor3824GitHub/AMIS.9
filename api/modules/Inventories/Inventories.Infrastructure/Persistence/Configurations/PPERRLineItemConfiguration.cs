using AMIS.WebApi.Inventories.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMIS.WebApi.Inventories.Infrastructure.Persistence.Configurations;

public sealed class PPERRLineItemConfiguration : IEntityTypeConfiguration<PPERRLineItem>
{
    public void Configure(EntityTypeBuilder<PPERRLineItem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable(nameof(PPERRLineItem));

        // Required properties
        builder.Property(x => x.PropertyCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.RRNumber).IsRequired().HasMaxLength(50);
    }
}
