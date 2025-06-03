using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateAcceptanceItemEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceItemCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAcceptanceItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateAcceptanceItemEndpoint))
            .WithSummary("creates a acceptance item")
            .WithDescription("creates a acceptance item")
            .Produces<CreateAcceptanceItemResponse>()
            .RequirePermission("Permissions.AcceptanceItems.Create")
            .MapToApiVersion(1);
    }
}
