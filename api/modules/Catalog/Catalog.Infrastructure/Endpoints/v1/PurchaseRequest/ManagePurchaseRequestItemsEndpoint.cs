using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class ManagePurchaseRequestItemsEndpoint
{
    internal static void MapPurchaseRequestItemManagementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost("/{id:guid}/items", async (Guid id, AddPurchaseRequestItemCommand body, ISender mediator) =>
            {
                // ensure route id and body PR id align
                var cmd = body with { PurchaseRequestId = id };
                var itemId = await mediator.Send(cmd);
                return Results.Ok(new { ItemId = itemId });
            })
            .WithSummary("add purchase request item")
            .RequirePermission("Permissions.PurchaseRequestItems.Create")
            .MapToApiVersion(1);

        endpoints
            .MapPut("/{id:guid}/items/{itemId:guid}", async (Guid id, Guid itemId, UpdatePurchaseRequestItemCommand body, ISender mediator) =>
            {
                var cmd = body with { PurchaseRequestId = id, ItemId = itemId };
                await mediator.Send(cmd);
                return Results.NoContent();
            })
            .WithSummary("update purchase request item")
            .RequirePermission("Permissions.PurchaseRequestItems.Update")
            .MapToApiVersion(1);

        endpoints
            .MapDelete("/{id:guid}/items/{itemId:guid}", async (Guid id, Guid itemId, ISender mediator) =>
            {
                await mediator.Send(new DeletePurchaseRequestItemCommand(id, itemId));
                return Results.NoContent();
            })
            .WithSummary("delete purchase request item")
            .RequirePermission("Permissions.PurchaseRequestItems.Delete")
            .MapToApiVersion(1);
    }
}
