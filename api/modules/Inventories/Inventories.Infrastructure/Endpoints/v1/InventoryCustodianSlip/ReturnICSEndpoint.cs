using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Return.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class ReturnICSEndpoint
{
    public static RouteHandlerBuilder MapReturnICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/return", async (Guid id, ReturnICSCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(ReturnICSEndpoint))
        .WithSummary("Return Inventory Custodian Slip")
        .WithDescription("Marks an Inventory Custodian Slip as returned, unassigning semi-expendable assets from custodian.")
        .Produces<ReturnICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Return")
        .MapToApiVersion(1);
    }
}
