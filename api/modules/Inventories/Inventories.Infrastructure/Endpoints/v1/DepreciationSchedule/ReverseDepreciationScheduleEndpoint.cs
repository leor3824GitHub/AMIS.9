using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Reverse.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

/// <summary>
/// Endpoint to reverse a depreciation schedule entry
/// </summary>
public static class ReverseDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapDepreciationScheduleReverseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/reverse", async (Guid id, [FromBody] ReverseDepreciationScheduleCommand command, ISender mediator) =>
            {
                var reverseCommand = new ReverseDepreciationScheduleCommand { Id = id, Reason = command.Reason };
                var response = await mediator.Send(reverseCommand);
                return Results.Ok(response);
            })
            .WithName(nameof(ReverseDepreciationScheduleEndpoint))
            .WithSummary("Reverses a depreciation schedule")
            .WithDescription("Reverses a posted depreciation schedule entry (marks as Reversed)")
            .Produces<ReverseDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.Reverse")
            .MapToApiVersion(1);
    }
}

