using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class UpdateICSEndpoint
{
    public static RouteHandlerBuilder MapUpdateICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id}", async (Guid id, UpdateICSCommand request, ISender mediator) =>
        {
            var command = request with { Id = id };
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateICSEndpoint))
        .WithSummary("Update Inventory Custodian Slip")
        .WithDescription("Updates an existing Inventory Custodian Slip (only allowed in Draft status).")
        .Produces<UpdateICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Update")
        .MapToApiVersion(1);
    }
}
