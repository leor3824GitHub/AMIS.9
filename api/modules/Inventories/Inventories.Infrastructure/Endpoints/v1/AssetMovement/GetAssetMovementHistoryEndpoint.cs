using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using AMIS.WebApi.Inventories.Application.AssetMovement.Queries;
using AMIS.WebApi.Inventories.Application.AssetMovement;
using AMIS.Framework.Infrastructure.Auth.Policy;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetMovement;

public sealed class GetAssetMovementHistoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/asset-movement")
            .WithTags("Asset Movement");

        group.MapGet("/{assetId:guid}", async (Guid assetId, ISender mediator) =>
        {
            var result = await mediator.Send(new GetAssetMovementHistoryQuery(assetId));
            return result is null
                ? Results.NotFound()
                : Results.Ok(result);
        })
        .WithName("GetAssetMovementHistory")
        .WithSummary("Get complete movement and custody history for an asset")
        .WithDescription("Returns all PAR, ICS, PPEIR, and SMIR documents that reference this asset, showing complete custody and transfer chain")
        .Produces<AssetMovementSummaryDto>()
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.Assets.View")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
