using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Get.v1;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetClassificationRules;

public static class SearchAssetClassificationRulesEndpoint
{
    internal static RouteHandlerBuilder MapSearchAssetClassificationRulesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAssetClassificationRulesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAssetClassificationRulesEndpoint))
            .WithSummary("Gets a list of asset classification rules")
            .WithDescription("Gets a list of asset classification rules with pagination and filtering support")
            .Produces<PagedList<AssetClassificationRuleResponse>>()
            .RequirePermission("Permissions.AssetClassificationRules.View")
            .MapToApiVersion(1);
    }
}
