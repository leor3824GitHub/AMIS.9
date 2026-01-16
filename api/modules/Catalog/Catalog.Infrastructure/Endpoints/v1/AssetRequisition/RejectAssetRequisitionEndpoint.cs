using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Reject.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AssetRequisition;

public static class RejectAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapRejectAssetRequisitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", RejectHandler)
            .WithName(nameof(RejectAssetRequisitionEndpoint))
            .Produces<RejectAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> RejectHandler(
        Guid id,
        [FromBody] RejectAssetRequisitionRequest request,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new RejectAssetRequisitionCommand(id, request.Reason);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

public sealed record RejectAssetRequisitionRequest(string Reason);
