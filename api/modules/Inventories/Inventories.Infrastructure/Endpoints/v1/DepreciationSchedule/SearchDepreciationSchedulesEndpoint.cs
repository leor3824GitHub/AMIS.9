using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.DepreciationSchedule;

public static class SearchDepreciationSchedulesEndpoint
{
    internal static RouteHandlerBuilder MapSearchDepreciationSchedulesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchDepreciationSchedulesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchDepreciationSchedulesEndpoint))
            .Produces<PagedList<DepreciationScheduleDto>>()
            .RequirePermission("Permissions.DepreciationSchedules.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> SearchHandler(
        ISender mediator,
        [FromBody] SearchDepreciationSchedulesCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

