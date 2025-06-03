using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateAcceptanceEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAcceptanceCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateAcceptanceEndpoint))
            .WithSummary("creates a acceptance")
            .WithDescription("creates a acceptance")
            .Produces<CreateAcceptanceResponse>()
            .RequirePermission("Permissions.Acceptances.Create")
            .MapToApiVersion(1);
    }
}
