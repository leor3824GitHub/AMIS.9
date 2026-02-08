using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetClassificationRules;

public static class GetAssetClassificationRuleEndpoint
{
    internal static RouteHandlerBuilder MapGetAssetClassificationRuleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAssetClassificationRuleRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetAssetClassificationRuleEndpoint))
            .WithSummary("Gets an asset classification rule by ID")
            .WithDescription("Retrieves a specific asset classification rule by its ID")
            .Produces<AssetClassificationRuleResponse>()
            .RequirePermission("Permissions.AssetClassificationRules.View")
            .MapToApiVersion(1);
    }
}
