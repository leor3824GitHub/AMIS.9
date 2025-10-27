using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetAcceptanceEndpoint
{
    internal static RouteHandlerBuilder MapGetAcceptanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAcceptanceRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetAcceptanceEndpoint))
            .WithSummary("gets acceptance by id")
            .WithDescription("gets acceptance by id")
            .Produces<AcceptanceResponse>()
            .RequirePermission("Permissions.Acceptances.View")
            .MapToApiVersion(1);
    }
}
