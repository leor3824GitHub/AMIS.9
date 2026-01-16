using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class CreatePhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapCreatePhysicalAssetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", CreateHandler)
            .WithName(nameof(CreatePhysicalAssetEndpoint))
            .Produces<CreatePhysicalAssetResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.PhysicalAssets.Create")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> CreateHandler(
        [FromBody] CreatePhysicalAssetCommand command,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Results.Created($"/catalog/physicalAssets/{response.Id}", response);
    }
}
