using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class SearchPurchaseRequestsEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseRequestListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/", async (int pageNumber, int pageSize, string? keyword, string[]? orderBy, Guid? requestedBy, string? status, ISender mediator) =>
            {
                Domain.ValueObjects.PurchaseRequestStatus? statusEnum = null;
                if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<Domain.ValueObjects.PurchaseRequestStatus>(status, true, out var parsed))
                {
                    statusEnum = parsed;
                }
                var cmd = new SearchPurchaseRequestsCommand
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    Keyword = keyword,
                    Status = statusEnum,
                    RequestedBy = requestedBy
                };
                var response = await mediator.Send(cmd);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPurchaseRequestsEndpoint))
            .WithSummary("search purchase requests")
            .WithDescription("search purchase requests with pagination and filtering")
            .RequirePermission("Permissions.PurchaseRequests.Search")
            .MapToApiVersion(1);
    }
}
