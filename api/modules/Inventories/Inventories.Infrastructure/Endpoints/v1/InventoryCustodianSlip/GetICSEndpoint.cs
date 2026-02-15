using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class GetICSEndpoint
{
    public static RouteHandlerBuilder MapGetICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}", async (Guid id, ISender mediator) =>
        {
            var query = new GetICSQuery(id);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        })
        .WithName(nameof(GetICSEndpoint))
        .WithSummary("Get Inventory Custodian Slip")
        .WithDescription("Retrieves a specific Inventory Custodian Slip by ID.")
        .Produces<GetICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Read")
        .MapToApiVersion(1);
    }
}
