using AMIS.Framework.Core.Tenant.Abstractions;
using AMIS.Framework.Core.Tenant.Dtos;
using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.GetTenants;
public sealed class GetTenantsHandler(ITenantService service) : IRequestHandler<GetTenantsQuery, List<TenantDetail>>
{
    public Task<List<TenantDetail>> Handle(GetTenantsQuery request, CancellationToken cancellationToken)
    {
        return service.GetAllAsync();
    }
}
