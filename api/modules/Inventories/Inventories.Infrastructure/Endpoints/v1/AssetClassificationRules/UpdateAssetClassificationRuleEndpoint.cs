using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AssetClassificationRules.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetClassificationRules;

public static class UpdateAssetClassificationRuleEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAssetClassificationRuleEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateAssetClassificationRuleCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAssetClassificationRuleEndpoint))
            .WithSummary("Updates an asset classification rule")
            .WithDescription("Updates an existing asset classification rule")
            .Produces<UpdateAssetClassificationRuleResponse>()
            .RequirePermission("Permissions.AssetClassificationRules.Update")
            .MapToApiVersion(1);
    }
}
