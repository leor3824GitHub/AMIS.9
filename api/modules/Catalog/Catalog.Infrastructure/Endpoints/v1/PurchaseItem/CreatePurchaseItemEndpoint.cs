using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreatePurchaseItemEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseItemCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreatePurchaseItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreatePurchaseItemEndpoint))
            .WithSummary("creates a purchaseItem")
            .WithDescription("creates a purchaseItem")
            .Produces<CreatePurchaseItemResponse>()
            .RequirePermission("Permissions.PurchaseItems.Create")
            .MapToApiVersion(1);
    }
}
