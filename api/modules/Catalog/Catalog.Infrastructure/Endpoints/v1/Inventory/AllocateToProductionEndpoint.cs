using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.AllocateToProduction.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class AllocateToProductionEndpoint
{
    internal static RouteHandlerBuilder MapAllocateToProductionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/allocate-to-production", async (Guid id, AllocationRequest request, ISender mediator) =>
            {
                var command = new AllocateToProductionCommand(id, request.Quantity);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(AllocateToProductionEndpoint))
            .WithSummary("Allocate inventory to production")
            .WithDescription("Allocates specified quantity to production")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<AllocateToProductionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record AllocationRequest(int Quantity);
