using AMIS.Framework.Core.Tenant.Dtos;
using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.GetTenantById;
public record GetTenantByIdQuery(string TenantId) : IRequest<TenantDetail>;
