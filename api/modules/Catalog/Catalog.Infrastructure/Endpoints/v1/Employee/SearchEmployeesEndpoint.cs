using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchEmployeesEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchEmployeesCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchEmployeesEndpoint");
                logger.LogInformation(
                    "SearchEmployees requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchEmployees result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchEmployeesEndpoint))
            .WithSummary("Gets a list of employees")
            .WithDescription("Gets a list of employees with pagination and filtering support")
            .Produces<PagedList<EmployeeResponse>>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}
