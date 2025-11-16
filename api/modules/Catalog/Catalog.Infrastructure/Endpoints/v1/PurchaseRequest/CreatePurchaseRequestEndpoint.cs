using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreatePurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseRequestCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePurchaseRequestCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePurchaseRequestEndpoint))
            .WithSummary("creates a purchase request")
            .WithDescription("creates a purchase request with optional items")
            .Produces<CreatePurchaseRequestResponse>()
            .RequirePermission("Permissions.PurchaseRequests.Create")
            .MapToApiVersion(1);
    }
}
