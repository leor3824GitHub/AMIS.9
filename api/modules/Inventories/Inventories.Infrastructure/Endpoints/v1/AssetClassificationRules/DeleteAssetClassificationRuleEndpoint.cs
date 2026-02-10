using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetClassificationRules;

public static class DeleteAssetClassificationRuleEndpoint
{
    internal static RouteHandlerBuilder MapDeleteAssetClassificationRuleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new DeleteAssetClassificationRuleCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(DeleteAssetClassificationRuleEndpoint))
            .WithSummary("Deletes an asset classification rule")
            .WithDescription("Deletes an asset classification rule by its ID")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.AssetClassificationRules.Delete")
            .MapToApiVersion(1);
    }
}
