using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetIssuanceItemEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetIssuanceItemRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetIssuanceItemEndpoint))
            .WithSummary("gets issuanceItem by id")
            .WithDescription("gets issuanceItem by id")
            .Produces<IssuanceItemResponse>()
            .RequirePermission("Permissions.IssuanceItems.View")
            .MapToApiVersion(1);
    }
}
