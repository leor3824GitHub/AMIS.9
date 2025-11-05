using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Application.Suppliers.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchSuppliersEndpoint
{
    internal static RouteHandlerBuilder MapGetSupplierListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSuppliersCommand command, ILoggerFactory loggerFactory) =>
            {
                var logger = loggerFactory.CreateLogger("SearchSuppliersEndpoint");
                logger.LogInformation(
                    "SearchSuppliers requested: PageNumber={PageNumber}, PageSize={PageSize}, HasKeyword={HasKeyword}, HasAdvancedSearch={HasAdvancedSearch}, HasAdvancedFilter={HasAdvancedFilter}",
                    command.PageNumber,
                    command.PageSize,
                    !string.IsNullOrWhiteSpace(command.Keyword),
                    command.AdvancedSearch is not null,
                    command.AdvancedFilter is not null);

                var response = await mediator.Send(command);

                logger.LogInformation(
                    "SearchSuppliers result: TotalCount={Total}, Returned={Returned}",
                    response.TotalCount,
                    response.Items?.Count ?? 0);

                return Results.Ok(response);
            })
            .WithName(nameof(SearchSuppliersEndpoint))
            .WithSummary("Gets a list of suppliers")
            .WithDescription("Gets a list of suppliers with pagination and filtering support")
            .Produces<PagedList<SupplierResponse>>()
            .RequirePermission("Permissions.Suppliers.View")
            .MapToApiVersion(1);
    }
}
