using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetPurchaseItemEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPurchaseItemRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetPurchaseItemEndpoint))
            .WithSummary("gets purchaseItem by id")
            .WithDescription("gets purchaseItem by id")
            .Produces<PurchaseItemResponse>()
            .RequirePermission("Permissions.PurchaseItems.View")
            .MapToApiVersion(1);
    }
}
