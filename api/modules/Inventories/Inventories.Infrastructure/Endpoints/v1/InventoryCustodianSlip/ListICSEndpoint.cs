using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.List.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class ListICSEndpoint
{
    public static RouteHandlerBuilder MapListICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (int pageNumber = 1, int pageSize = 10, ISender mediator = default) =>
        {
            var query = new ListICSQuery(pageNumber, pageSize);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        })
        .WithName(nameof(ListICSEndpoint))
        .WithSummary("List Inventory Custodian Slips")
        .WithDescription("Retrieves a list of all Inventory Custodian Slips with pagination.")
        .Produces<ListICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Read")
        .MapToApiVersion(1);
    }
}
