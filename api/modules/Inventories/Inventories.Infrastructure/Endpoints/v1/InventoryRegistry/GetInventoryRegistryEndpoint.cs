using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryRegistry;

public static class GetInventoryRegistryEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryRegistryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInventoryRegistryRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryRegistryEndpoint))
            .WithSummary("Gets an inventory registry entry by id")
            .WithDescription("Gets an inventory registry entry by id")
            .Produces<InventoryRegistryResponse>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
