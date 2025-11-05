using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Inspections.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;

public static class SearchInspectionsEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInspectionsCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchInspectionsEndpoint");
                // Log only non-sensitive metadata about the request
                logger.LogInformation(
                    "SearchInspections requested: PageNumber={PageNumber}, PageSize={PageSize}, HasPurchaseId={HasPurchaseId}, HasInspectorId={HasInspectorId}, HasFromDate={HasFromDate}, HasToDate={HasToDate}",
                    command.PageNumber,
                    command.PageSize,
                    command.PurchaseId.HasValue,
                    command.InspectorId.HasValue,
                    command.FromDate.HasValue,
                    command.ToDate.HasValue);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchInspections result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchInspectionsEndpoint))
            .WithSummary("Gets a list of inspections")
            .WithDescription("Gets a list of inspections with pagination and filtering support")
            .Produces<PagedList<InspectionResponse>>()
            .RequirePermission("Permissions.Inspections.View")
            .MapToApiVersion(1);
    }
}

