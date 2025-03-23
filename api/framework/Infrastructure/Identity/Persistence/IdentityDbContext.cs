using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using AMIS.Framework.Core.Audit;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Infrastructure.Identity.RoleClaims;
using AMIS.Framework.Infrastructure.Identity.Roles;
using AMIS.Framework.Infrastructure.Identity.Users;
using AMIS.Framework.Infrastructure.Persistence;
using AMIS.Framework.Infrastructure.Tenant;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AMIS.Framework.Infrastructure.Identity.Persistence;
public class IdentityDbContext : MultiTenantIdentityDbContext<FshUser,
    FshRole,
    string,
    IdentityUserClaim<string>,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    FshRoleClaim,
    IdentityUserToken<string>>
{
    private readonly DatabaseOptions _settings;
    private new FshTenantInfo TenantInfo { get; set; }
    public IdentityDbContext(IMultiTenantContextAccessor<FshTenantInfo> multiTenantContextAccessor, DbContextOptions<IdentityDbContext> options, IOptions<DatabaseOptions> settings) : base(multiTenantContextAccessor, options)
    {
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }

    public DbSet<AuditTrail> AuditTrails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.ConfigureDatabase(_settings.Provider, TenantInfo.ConnectionString);
        }
    }
}
