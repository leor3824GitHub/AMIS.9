using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class CancelICSEndpoint
{
    public static RouteHandlerBuilder MapCancelICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/cancel", async (Guid id, ISender mediator) =>
        {
            var command = new CancelICSCommand(id);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(CancelICSEndpoint))
        .WithSummary("Cancel Inventory Custodian Slip")
        .WithDescription("Cancels an Inventory Custodian Slip, reversing custodian assignments.")
        .Produces<CancelICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Cancel")
        .MapToApiVersion(1);
    }
}
