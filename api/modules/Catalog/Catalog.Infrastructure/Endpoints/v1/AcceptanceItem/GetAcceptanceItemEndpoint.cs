using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetAcceptanceItemEndpoint
{
    internal static RouteHandlerBuilder MapGetAcceptanceItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAcceptanceItemRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetAcceptanceItemEndpoint))
            .WithSummary("gets acceptance item by id")
            .WithDescription("gets acceptance item by id")
            .Produces<AcceptanceItemResponse>()
            .RequirePermission("Permissions.AcceptanceItems.View")
            .MapToApiVersion(1);
    }
}
