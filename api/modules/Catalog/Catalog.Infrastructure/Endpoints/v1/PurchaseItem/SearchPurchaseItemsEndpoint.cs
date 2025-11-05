using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchPurchaseItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchPurchaseItemsCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchPurchaseItemsEndpoint");
                logger.LogInformation(
                    "SearchPurchaseItems requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchPurchaseItems result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

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

