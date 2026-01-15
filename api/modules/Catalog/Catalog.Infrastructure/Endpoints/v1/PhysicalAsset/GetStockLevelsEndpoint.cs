using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.GetStockLevels.v1;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class GetStockLevelsEndpoint
{
    public static void MapGetStockLevelsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/stock-levels", async (ISender mediator) =>
        {
            var query = new GetStockLevelsQuery();
            var response = await mediator.Send(query);
            return Results.Ok(response);
        })
        .WithName(nameof(GetStockLevelsEndpoint))
        .WithSummary("Get physical asset stock levels")
        .WithDescription("Retrieves aggregated stock levels and distribution statistics for physical assets")
        .Produces<GetStockLevelsResponse>()
        .RequirePermission("Permissions.PhysicalAssets.View")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
