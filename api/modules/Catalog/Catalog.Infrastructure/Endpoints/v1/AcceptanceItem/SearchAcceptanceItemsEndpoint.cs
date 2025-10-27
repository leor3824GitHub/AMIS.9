using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchAcceptanceItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetAcceptanceItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAcceptanceItemsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAcceptanceItemsEndpoint))
            .WithSummary("Gets a list of acceptance items")
            .WithDescription("Gets a list of acceptance items with pagination and filtering support")
            .Produces<PagedList<AcceptanceItemResponse>>()
            .RequirePermission("Permissions.AcceptanceItems.View")
            .MapToApiVersion(1);
    }
}

