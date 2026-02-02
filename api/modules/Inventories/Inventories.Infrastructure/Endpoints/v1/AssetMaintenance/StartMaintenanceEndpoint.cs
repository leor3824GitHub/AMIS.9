using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Maintenance.Start.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetMaintenance;

public static class StartMaintenanceEndpoint
{
    internal static RouteHandlerBuilder MapStartMaintenanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/start", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new StartMaintenanceCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(StartMaintenanceEndpoint))
            .WithSummary("Start maintenance work")
            .WithDescription("Mark scheduled maintenance as in-progress. Begin actual maintenance work.")
            .Produces<StartMaintenanceResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Maintenance.Start")
            .MapToApiVersion(1);
    }
}
