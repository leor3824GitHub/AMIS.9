using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetRequisitions.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetRequisition;

public static class SearchAssetRequisitionsEndpoint
{
    internal static RouteHandlerBuilder MapSearchAssetRequisitionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAssetRequisitionsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAssetRequisitionsEndpoint))
            .Produces<PagedList<AssetRequisitionDto>>()
            .RequirePermission("Permissions.AssetRequisitions.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> SearchHandler(
        ISender mediator,
        SearchAssetRequisitionsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

