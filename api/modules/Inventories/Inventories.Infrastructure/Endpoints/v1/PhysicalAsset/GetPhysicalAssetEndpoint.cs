using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class GetPhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapGetPhysicalAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", GetHandler)
            .WithName(nameof(GetPhysicalAssetEndpoint))
            .Produces<PhysicalAssetResponse>()
            .RequirePermission("Permissions.PhysicalAssets.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GetPhysicalAssetCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

