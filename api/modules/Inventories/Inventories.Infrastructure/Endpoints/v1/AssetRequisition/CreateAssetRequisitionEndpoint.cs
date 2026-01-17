using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetRequisitions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetRequisition;

public static class CreateAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapAssetRequisitionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAssetRequisitionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateAssetRequisitionEndpoint))
            .WithSummary("Creates an asset requisition")
            .WithDescription("Creates an asset requisition for an issuance")
            .Produces<CreateAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.Create")
            .MapToApiVersion(1);
    }
}

