using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Maintenance.Complete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetMaintenance;

public static class CompleteMaintenanceEndpoint
{
    internal static RouteHandlerBuilder MapCompleteMaintenanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (Guid id, CompleteMaintenanceRequest request, ISender mediator) =>
            {
                var command = new CompleteMaintenanceCommand(id, request.CompletionNotes, request.FindingsNotes, request.ActualCost);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CompleteMaintenanceEndpoint))
            .WithSummary("Complete maintenance")
            .WithDescription("Mark maintenance as completed. Records findings, notes, and actual cost.")
            .Produces<CompleteMaintenanceResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Maintenance.Complete")
            .MapToApiVersion(1);
    }
}

public record CompleteMaintenanceRequest(
    string? CompletionNotes = null,
    string? FindingsNotes = null,
    decimal? ActualCost = null);
