using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Inventories.Application.PurchaseRequests.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PurchaseRequest;
public static class SearchPurchaseRequestsEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseRequestListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchPurchaseRequestsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPurchaseRequestsEndpoint))
            .WithSummary("search purchase requests")
            .WithDescription("search purchase requests with pagination and filtering")
            .Produces<PagedList<PurchaseRequestResponse>>()
            .RequirePermission("Permissions.PurchaseRequests.Search")
            .MapToApiVersion(1);
    }
}

