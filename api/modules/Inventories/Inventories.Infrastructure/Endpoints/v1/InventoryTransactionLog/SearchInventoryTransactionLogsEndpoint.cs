using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;
using AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryTransactionLog;

public static class SearchInventoryTransactionLogsEndpoint
{
    internal static RouteHandlerBuilder MapSearchInventoryTransactionLogsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInventoryTransactionLogsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoryTransactionLogsEndpoint))
            .WithSummary("Search inventory transaction logs")
            .WithDescription("Search inventory transaction logs with pagination and filtering")
            .Produces<PagedList<InventoryTransactionLogResponse>>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
