using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.SetCostingMethod.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SetCostingMethodEndpoint
{
    internal static RouteHandlerBuilder MapSetCostingMethodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/set-costing-method", async (Guid id, CostingMethodRequest request, ISender mediator) =>
            {
                var command = new SetCostingMethodCommand(id, request.Method);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SetCostingMethodEndpoint))
            .WithSummary("Set inventory costing method")
            .WithDescription("Updates the costing method (FIFO, LIFO, Average) for inventory valuation")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<SetCostingMethodResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record CostingMethodRequest(CostingMethod Method);
