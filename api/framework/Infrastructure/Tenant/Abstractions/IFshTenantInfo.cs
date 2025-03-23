using Finbuckle.MultiTenant.Abstractions;

namespace AMIS.Framework.Infrastructure.Tenant.Abstractions;
public interface IFshTenantInfo : ITenantInfo
{
    string? ConnectionString { get; set; }
}
