using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchAcceptancesEndpoint
{
    internal static RouteHandlerBuilder MapGetAcceptanceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAcceptancesCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchAcceptancesEndpoint");
                logger.LogInformation(
                    "SearchAcceptances requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchAcceptances result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchAcceptancesEndpoint))
            .WithSummary("Gets a list of acceptances")
            .WithDescription("Gets a list of acceptances with pagination and filtering support")
            .Produces<PagedList<AcceptanceResponse>>()
            .RequirePermission("Permissions.Acceptances.View")
            .MapToApiVersion(1);
    }
}

