using Carter;
using AMIS.WebApi.Catalog.Application.Acceptances.ManageItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class AcceptanceItemManagementEndpoints
{
    internal static void MapAcceptanceItemManagementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/acceptances/{acceptanceId:guid}/items").WithTags("acceptanceItems");

        group.MapPost("/", async (Guid acceptanceId, AddAcceptanceItemCommand command, ISender mediator) =>
        {
            if (command.AcceptanceId != acceptanceId) return Results.BadRequest();
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("AddAcceptanceItem")
        .WithSummary("Add item to acceptance")
        .WithDescription("Adds a new item to an acceptance")
        .MapToApiVersion(1);

        group.MapPut("/{itemId:guid}", async (Guid acceptanceId, Guid itemId, UpdateAcceptanceItemCommand command, ISender mediator) =>
        {
            if (command.AcceptanceId != acceptanceId || command.ItemId != itemId) return Results.BadRequest();
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateAcceptanceItem")
        .WithSummary("Update item in acceptance")
        .WithDescription("Updates an item in an acceptance")
        .MapToApiVersion(1);

        group.MapDelete("/{itemId:guid}", async (Guid acceptanceId, Guid itemId, ISender mediator) =>
        {
            await mediator.Send(new RemoveAcceptanceItemCommand(acceptanceId, itemId));
            return Results.NoContent();
        })
        .WithName("RemoveAcceptanceItem")
        .WithSummary("Remove item from acceptance")
        .WithDescription("Removes an item from an acceptance")
        .MapToApiVersion(1);
    }
}
