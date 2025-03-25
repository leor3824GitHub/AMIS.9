using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreatePurchaseEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePurchaseCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePurchaseEndpoint))
            .WithSummary("creates a purchase")
            .WithDescription("creates a purchase")
            .Produces<CreatePurchaseResponse>()
            .RequirePermission("Permissions.Purchases.Create")
            .MapToApiVersion(1);
    }
}
