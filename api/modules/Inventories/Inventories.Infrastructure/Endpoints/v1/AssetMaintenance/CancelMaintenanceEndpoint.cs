using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Maintenance.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetMaintenance;

public static class CancelMaintenanceEndpoint
{
    internal static RouteHandlerBuilder MapCancelMaintenanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, CancelMaintenanceRequest request, ISender mediator) =>
            {
                var command = new CancelMaintenanceCommand(id, request.CancellationReason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelMaintenanceEndpoint))
            .WithSummary("Cancel maintenance")
            .WithDescription("Cancel scheduled or in-progress maintenance.")
            .Produces<CancelMaintenanceResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Maintenance.Cancel")
            .MapToApiVersion(1);
    }
}

public record CancelMaintenanceRequest(string? CancellationReason = null);
