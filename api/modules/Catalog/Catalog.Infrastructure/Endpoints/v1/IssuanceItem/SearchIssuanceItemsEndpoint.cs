using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchIssuanceItemsEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceItemListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchIssuanceItemsCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchIssuanceItemsEndpoint");
                logger.LogInformation(
                    "SearchIssuanceItems requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchIssuanceItems result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

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

