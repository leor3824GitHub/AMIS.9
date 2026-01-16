using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class DeletePhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapDeletePhysicalAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", DeleteHandler)
            .WithName(nameof(DeletePhysicalAssetEndpoint))
            .Produces<DeletePhysicalAssetResponse>()
            .RequirePermission("Permissions.PhysicalAssets.Delete")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> DeleteHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeletePhysicalAssetCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}
