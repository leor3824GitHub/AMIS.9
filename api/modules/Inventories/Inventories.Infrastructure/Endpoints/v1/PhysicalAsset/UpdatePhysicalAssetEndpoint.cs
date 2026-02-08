using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class UpdatePhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePhysicalAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", UpdateHandler)
            .WithName(nameof(UpdatePhysicalAssetEndpoint))
            .Produces<UpdatePhysicalAssetResponse>()
            .RequirePermission("Permissions.PhysicalAssets.Update")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> UpdateHandler(
        Guid id,
        UpdatePhysicalAssetCommand command,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var updateCommand = new UpdatePhysicalAssetCommand(id, command.Condition, command.ParentAssetId);
        var response = await mediator.Send(updateCommand, cancellationToken);
        return Results.Ok(response);
    }
}

