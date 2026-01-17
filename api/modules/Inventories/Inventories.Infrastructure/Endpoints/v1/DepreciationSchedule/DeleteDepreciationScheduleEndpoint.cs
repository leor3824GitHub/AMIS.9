using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class DeleteDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapDeleteDepreciationScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", DeleteHandler)
            .WithName(nameof(DeleteDepreciationScheduleEndpoint))
            .Produces<DeleteDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> DeleteHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteDepreciationScheduleCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

