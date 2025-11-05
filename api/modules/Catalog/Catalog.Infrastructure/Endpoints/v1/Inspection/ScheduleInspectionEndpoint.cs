using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Schedule.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ScheduleInspectionEndpoint
{
    internal static RouteHandlerBuilder MapScheduleInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/schedule", async (Guid id, ScheduleRequest request, ISender mediator) =>
            {
                var command = new ScheduleInspectionCommand(id, request.ScheduledDate);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ScheduleInspectionEndpoint))
            .WithSummary("Schedule inspection")
            .WithDescription("Schedules an inspection for a specific date")
            .Produces<ScheduleInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}

public sealed record ScheduleRequest(DateTime ScheduledDate);
