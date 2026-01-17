using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Issuances.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Issuance;
public static class CreateIssuanceEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateIssuanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateIssuanceEndpoint))
            .WithSummary("creates a issuance")
            .WithDescription("creates a issuance")
            .Produces<CreateIssuanceResponse>()
            .RequirePermission("Permissions.Issuances.Create")
            .MapToApiVersion(1);
    }
}

