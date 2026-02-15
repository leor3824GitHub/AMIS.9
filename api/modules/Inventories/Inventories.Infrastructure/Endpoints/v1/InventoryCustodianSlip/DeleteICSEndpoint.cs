using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class DeleteICSEndpoint
{
    public static RouteHandlerBuilder MapDeleteICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id}", async (Guid id, ISender mediator) =>
        {
            var command = new DeleteICSCommand(id);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(DeleteICSEndpoint))
        .WithSummary("Delete Inventory Custodian Slip")
        .WithDescription("Deletes an Inventory Custodian Slip.")
        .Produces<DeleteICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Delete")
        .MapToApiVersion(1);
    }
}
