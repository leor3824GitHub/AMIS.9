using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Maintenance.Schedule.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetMaintenance;

public static class ScheduleMaintenanceEndpoint
{
    internal static RouteHandlerBuilder MapScheduleMaintenanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("", async (ScheduleMaintenanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Created($"/api/maintenance/{response.MaintenanceId}", response);
            })
            .WithName(nameof(ScheduleMaintenanceEndpoint))
            .WithSummary("Schedule maintenance")
            .WithDescription("Schedule maintenance for an asset. Supports preventive, corrective, and emergency maintenance.")
            .Produces<ScheduleMaintenanceResponse>(StatusCodes.Status201Created)
            .WithOpenApi()
            .RequirePermission("Permissions.Maintenance.Schedule")
            .MapToApiVersion(1);
    }
}
