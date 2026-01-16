using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AssetRequisition;

public static class GetAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapGetAssetRequisitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", GetHandler)
            .WithName(nameof(GetAssetRequisitionEndpoint))
            .Produces<GetAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GetAssetRequisitionCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}
