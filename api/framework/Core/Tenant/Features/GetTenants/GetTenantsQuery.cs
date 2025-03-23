using AMIS.Framework.Core.Tenant.Dtos;
using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.GetTenants;
public sealed class GetTenantsQuery : IRequest<List<TenantDetail>>;
