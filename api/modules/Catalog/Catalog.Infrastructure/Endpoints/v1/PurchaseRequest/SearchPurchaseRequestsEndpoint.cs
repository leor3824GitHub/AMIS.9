using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
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
