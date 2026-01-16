using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Accept.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AssetRequisition;

public static class AcceptAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapAssetRequisitionAcceptanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/accept", async (Guid id, AcceptAssetRequisitionCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command with { Id = id });
                return Results.Ok(response);
            })
            .WithName(nameof(AcceptAssetRequisitionEndpoint))
            .WithSummary("Accepts an asset requisition")
            .WithDescription("Accepts an asset requisition with digital signature")
            .Produces<AcceptAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.Accept")
            .MapToApiVersion(1);
    }
}
