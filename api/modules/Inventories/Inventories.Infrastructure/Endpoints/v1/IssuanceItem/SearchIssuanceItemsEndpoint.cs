using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.IssuanceItems.Get.v1;
using AMIS.WebApi.Inventories.Application.IssuanceItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.IssuanceItem;

public static class SearchIssuanceItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchIssuanceItemsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchIssuanceItemsEndpoint))
            .WithSummary("Gets a list of issuanceItems")
            .WithDescription("Gets a list of issuanceItems with pagination and filtering support")
            .Produces<PagedList<IssuanceItemResponse>>()
            .RequirePermission("Permissions.IssuanceItems.View")
            .MapToApiVersion(1);
    }
}


