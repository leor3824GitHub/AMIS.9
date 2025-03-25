using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchPurchaseItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchPurchaseItemsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPurchaseItemsEndpoint))
            .WithSummary("Gets a list of purchaseItems")
            .WithDescription("Gets a list of purchaseItems with pagination and filtering support")
            .Produces<PagedList<PurchaseItemResponse>>()
            .RequirePermission("Permissions.PurchaseItems.View")
            .MapToApiVersion(1);
    }
}

