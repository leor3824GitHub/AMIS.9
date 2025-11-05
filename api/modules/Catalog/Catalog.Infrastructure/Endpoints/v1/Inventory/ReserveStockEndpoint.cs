using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.ReserveStock.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReserveStockEndpoint
{
    internal static RouteHandlerBuilder MapReserveStockEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reserve", async (Guid id, ReserveStockRequest request, ISender mediator) =>
            {
                var command = new ReserveStockCommand(id, request.Quantity, request.ReservationReference);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReserveStockEndpoint))
            .WithSummary("Reserve inventory stock")
            .WithDescription("Reserves a specific quantity of inventory for a future transaction")
            .Produces<ReserveStockResponse>()
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(1);
    }
}

public sealed record ReserveStockRequest(int Quantity, string? ReservationReference);
