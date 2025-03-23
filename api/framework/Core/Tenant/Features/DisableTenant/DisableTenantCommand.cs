using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.DisableTenant;
public record DisableTenantCommand(string TenantId) : IRequest<DisableTenantResponse>;
