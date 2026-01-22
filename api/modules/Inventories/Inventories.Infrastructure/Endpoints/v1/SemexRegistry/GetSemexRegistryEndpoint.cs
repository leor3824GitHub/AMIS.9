using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SemexRegistry.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SemexRegistry;

public static class GetSemexRegistryEndpoint
{
    internal static RouteHandlerBuilder MapGetSemexRegistryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (Guid id, ISender mediator) =>
            {
                var query = new GetSemexRegistryByIdQuery(id);
                var response = await mediator.Send(query);
                return Results.Ok(response);
            })
            .WithName(nameof(GetSemexRegistryEndpoint))
            .WithSummary("Get semi-expendable inventory registry entry by ID")
            .WithDescription("Retrieves a specific semi-expendable inventory registry entry by its ID")
            .Produces<SemexRegistryDetailResponse>()
            .Produces(404)
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
