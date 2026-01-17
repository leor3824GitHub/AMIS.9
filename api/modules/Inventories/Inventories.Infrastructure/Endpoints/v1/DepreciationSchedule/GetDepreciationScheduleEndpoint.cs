using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class GetDepreciationScheduleEndpoint
{
    internal static RouteHandlerBuilder MapGetDepreciationScheduleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", GetHandler)
            .WithName(nameof(GetDepreciationScheduleEndpoint))
            .Produces<GetDepreciationScheduleResponse>()
            .RequirePermission("Permissions.DepreciationSchedules.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GetDepreciationScheduleCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

