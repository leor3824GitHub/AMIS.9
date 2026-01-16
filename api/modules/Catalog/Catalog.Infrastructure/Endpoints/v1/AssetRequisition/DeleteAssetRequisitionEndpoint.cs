using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AssetRequisition;

public static class DeleteAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapDeleteAssetRequisitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", DeleteHandler)
            .WithName(nameof(DeleteAssetRequisitionEndpoint))
            .Produces<DeleteAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> DeleteHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new DeleteAssetRequisitionCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}
