using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetIssuanceEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetIssuanceRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetIssuanceEndpoint))
            .WithSummary("gets issuance by id")
            .WithDescription("gets issuance by id")
            .Produces<IssuanceResponse>()
            .RequirePermission("Permissions.Issuances.View")
            .MapToApiVersion(1);
    }
}
