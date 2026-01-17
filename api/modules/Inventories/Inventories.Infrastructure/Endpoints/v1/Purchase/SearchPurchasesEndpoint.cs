using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
using AMIS.WebApi.Inventories.Application.Purchases.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Purchase;

public static class SearchPurchasesEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchPurchasesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPurchasesEndpoint))
            .WithSummary("Gets a list of purchases")
            .WithDescription("Gets a list of purchases with pagination and filtering support")
            .Produces<PagedList<PurchaseResponse>>()
            .RequirePermission("Permissions.Purchases.View")
            .MapToApiVersion(1);
    }
}


