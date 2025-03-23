using MediatR;

namespace AMIS.Framework.Core.Tenant.Features.ActivateTenant;
public record ActivateTenantCommand(string TenantId) : IRequest<ActivateTenantResponse>;
