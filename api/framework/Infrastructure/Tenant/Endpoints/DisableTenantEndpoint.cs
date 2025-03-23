using AMIS.Framework.Core.Tenant.Features.DisableTenant;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Framework.Infrastructure.Tenant.Endpoints;
public static class DisableTenantEndpoint
{
    internal static RouteHandlerBuilder MapDisableTenantEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/deactivate", (ISender mediator, string id) => mediator.Send(new DisableTenantCommand(id)))
                                .WithName(nameof(DisableTenantEndpoint))
                                .WithSummary("activate tenant")
                                .RequirePermission("Permissions.Tenants.Update")
                                .WithDescription("activate tenant");
    }
}
