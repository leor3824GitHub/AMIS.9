using Carter;
using AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class InspectionItemManagementEndpoints
{
    internal static void MapInspectionItemManagementEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/inspections/{inspectionId:guid}/items").WithTags("inspectionItems");

        group.MapPost("/", async (Guid inspectionId, AddInspectionItemCommand command, ISender mediator) =>
        {
            if (command.InspectionId != inspectionId) return Results.BadRequest();
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName("AddInspectionItem")
        .WithSummary("Add item to inspection")
        .WithDescription("Adds a new item to an inspection")
        .MapToApiVersion(1);

        group.MapPut("/{itemId:guid}", async (Guid inspectionId, Guid itemId, UpdateInspectionItemCommand command, ISender mediator) =>
        {
            if (command.InspectionId != inspectionId || command.ItemId != itemId) return Results.BadRequest();
            await mediator.Send(command);
            return Results.NoContent();
        })
        .WithName("UpdateInspectionItem")
        .WithSummary("Update item in inspection")
        .WithDescription("Updates an item in an inspection")
        .MapToApiVersion(1);

        group.MapDelete("/{itemId:guid}", async (Guid inspectionId, Guid itemId, ISender mediator) =>
        {
            await mediator.Send(new RemoveInspectionItemCommand(inspectionId, itemId));
            return Results.NoContent();
        })
        .WithName("RemoveInspectionItem")
        .WithSummary("Remove item from inspection")
        .WithDescription("Removes an item from an inspection")
        .MapToApiVersion(1);
    }
}
