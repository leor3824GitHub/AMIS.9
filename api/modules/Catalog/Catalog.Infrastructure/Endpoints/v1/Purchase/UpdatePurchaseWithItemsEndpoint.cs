using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class UpdatePurchaseWithItemsEndpoint
{
    internal static RouteHandlerBuilder MapUpdatePurchaseWithItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/with-items", async (Guid id, UpdatePurchaseWithItemsCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePurchaseWithItemsEndpoint))
            .WithSummary("Update a purchase header and its items atomically")
            .WithDescription("Upserts purchase items, applies deletions, and updates purchase header fields in a single operation")
            .Produces<UpdatePurchaseWithItemsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
