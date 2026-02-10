using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetClassificationRules;

public static class CreateAssetClassificationRuleEndpoint
{
    internal static RouteHandlerBuilder MapCreateAssetClassificationRuleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAssetClassificationRuleCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.CreatedAtRoute(
                    nameof(GetAssetClassificationRuleEndpoint),
                    new { id = response.Id },
                    response);
            })
            .WithName(nameof(CreateAssetClassificationRuleEndpoint))
            .WithSummary("Creates an asset classification rule")
            .WithDescription("Creates a new asset classification rule for cost-based asset categorization")
            .Produces<CreateAssetClassificationRuleResponse>()
            .RequirePermission("Permissions.AssetClassificationRules.Create")
            .MapToApiVersion(1);
    }
}
