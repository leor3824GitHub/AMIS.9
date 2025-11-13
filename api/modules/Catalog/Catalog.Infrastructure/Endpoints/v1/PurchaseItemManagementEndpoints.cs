using Carter;
using AMIS.WebApi.Catalog.Application.Purchases.ManageItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class PurchaseItemManagementEndpoints
{
    internal static void MapPurchaseItemManagementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/purchases/{purchaseId:guid}/items").WithTags("purchaseItems");

        group.MapPost("/", async (Guid purchaseId, AddPurchaseItemCommand command, ISender mediator) =>
        {
            if (command.PurchaseId != purchaseId) return Results.BadRequest();
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("AddPurchaseItem")
        .WithSummary("Add item to purchase")
        .WithDescription("Adds a new item to a purchase")
        .MapToApiVersion(1);

        group.MapPut("/{itemId:guid}", async (Guid purchaseId, Guid itemId, UpdatePurchaseItemCommand command, ISender mediator) =>
        {
            if (command.PurchaseId != purchaseId || command.ItemId != itemId) return Results.BadRequest();
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdatePurchaseItem")
        .WithSummary("Update item in purchase")
        .WithDescription("Updates an item in a purchase")
        .MapToApiVersion(1);

        group.MapDelete("/{itemId:guid}", async (Guid purchaseId, Guid itemId, ISender mediator) =>
        {
            await mediator.Send(new RemovePurchaseItemCommand(purchaseId, itemId));
            return Results.NoContent();
        })
        .WithName("RemovePurchaseItem")
        .WithSummary("Remove item from purchase")
        .WithDescription("Removes an item from a purchase")
        .MapToApiVersion(1);
    }
}
