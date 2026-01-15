using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AssetRequisitions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AssetRequisition;

public static class UpdateAssetRequisitionEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAssetRequisitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", UpdateHandler)
            .WithName(nameof(UpdateAssetRequisitionEndpoint))
            .Produces<UpdateAssetRequisitionResponse>()
            .RequirePermission("Permissions.AssetRequisitions.Edit")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> UpdateHandler(
        Guid id,
        UpdateAssetRequisitionCommand command,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var updateCommand = new UpdateAssetRequisitionCommand(id, command.ExpirationDate, command.Remarks);
        var response = await mediator.Send(updateCommand, cancellationToken);
        return Results.Ok(response);
    }
}
