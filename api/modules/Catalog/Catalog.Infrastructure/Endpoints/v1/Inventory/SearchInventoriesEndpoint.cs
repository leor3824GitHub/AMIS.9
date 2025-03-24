using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using AMIS.WebApi.Catalog.Application.Inventories.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchInventoriesEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInventoriesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoriesEndpoint))
            .WithSummary("Gets a list of inventorys")
            .WithDescription("Gets a list of inventorys with pagination and filtering support")
            .Produces<PagedList<InventoryResponse>>()
            .RequirePermission("Permissions.Inventories.View")
            .MapToApiVersion(1);
    }
}

