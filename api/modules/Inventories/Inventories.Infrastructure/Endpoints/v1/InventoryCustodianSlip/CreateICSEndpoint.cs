using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class CreateICSEndpoint
{
    public static RouteHandlerBuilder MapCreateICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateICSCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.CreatedAtRoute(nameof(GetICSEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreateICSEndpoint))
        .WithSummary("Create Inventory Custodian Slip")
        .WithDescription("Creates a new Inventory Custodian Slip (ICS) for assigning semi-expendable assets to custodians.")
        .Produces<CreateICSResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Create")
        .MapToApiVersion(1);
    }
}
