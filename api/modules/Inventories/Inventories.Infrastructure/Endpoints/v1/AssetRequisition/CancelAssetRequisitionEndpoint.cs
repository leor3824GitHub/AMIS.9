using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetRequisitions.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetRequisition;

public static class CancelAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapCancelAssetRequisitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", CancelHandler)
            .WithName(nameof(CancelAssetRequisitionEndpoint))
            .Produces<CancelAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> CancelHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new CancelAssetRequisitionCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

