using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Get.v1;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryRegistry;

public static class SearchInventoryRegistriesEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryRegistriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInventoryRegistriesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoryRegistriesEndpoint))
            .WithSummary("Search inventory registry entries")
            .WithDescription("Search inventory registry entries with pagination and filtering")
            .Produces<PagedList<InventoryRegistryResponse>>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
