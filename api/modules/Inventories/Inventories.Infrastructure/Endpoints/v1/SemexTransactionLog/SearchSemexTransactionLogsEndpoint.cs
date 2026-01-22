using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.SemexTransactionLog;

public static class SearchSemexTransactionLogsEndpoint
{
    internal static RouteHandlerBuilder MapSearchSemexTransactionLogsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchSemexTransactionLogsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchSemexTransactionLogsEndpoint))
            .WithSummary("Search semi-expendable inventory transaction logs")
            .WithDescription("Search semi-expendable inventory transaction logs with pagination and filtering")
            .Produces<PagedList<SemexTransactionLogResponse>>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
