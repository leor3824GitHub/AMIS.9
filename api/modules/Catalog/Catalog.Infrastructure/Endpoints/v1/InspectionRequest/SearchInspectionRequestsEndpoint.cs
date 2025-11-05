using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class SearchInspectionRequestsEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionRequestListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInspectionRequestsCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchInspectionRequestsEndpoint");
                logger.LogInformation(
                    "SearchInspectionRequests requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchInspectionRequests result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchInspectionRequestsEndpoint))
            .WithSummary("Gets a list of inspectionRequests")
            .WithDescription("Gets a list of inspectionRequests with pagination and filtering support")
            .Produces<PagedList<InspectionRequestResponse>>()
            .RequirePermission("Permissions.InspectionRequests.View")
            .MapToApiVersion(1);
    }
}

