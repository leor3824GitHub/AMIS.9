using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class SearchPhysicalAssetsEndpoint
{
    internal static RouteHandlerBuilder MapSearchPhysicalAssetsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", SearchHandler)
            .WithName(nameof(SearchPhysicalAssetsEndpoint))
            .Produces<PagedList<PhysicalAssetResponse>>()
            .RequirePermission("Permissions.PhysicalAssets.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> SearchHandler(
        ISender mediator,
        [FromBody] SearchPhysicalAssetsCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

