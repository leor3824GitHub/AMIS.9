using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Application.Issuances.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchIssuancesEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchIssuancesCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchIssuancesEndpoint");
                logger.LogInformation(
                    "SearchIssuances requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchIssuances result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchIssuancesEndpoint))
            .WithSummary("Gets a list of issuances")
            .WithDescription("Gets a list of issuances with pagination and filtering support")
            .Produces<PagedList<IssuanceResponse>>()
            .RequirePermission("Permissions.Issuances.View")
            .MapToApiVersion(1);
    }
}

