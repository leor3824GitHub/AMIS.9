using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SemexRegistry.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SemexRegistry;

public static class SearchSemexRegistriesEndpoint
{
    internal static RouteHandlerBuilder MapSearchSemexRegistriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSemexRegistriesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSemexRegistriesEndpoint))
            .WithSummary("Search semi-expendable inventory registry entries")
            .WithDescription("Search semi-expendable inventory registry entries with pagination and filtering")
            .Produces<PagedList<SemexRegistryResponse>>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
