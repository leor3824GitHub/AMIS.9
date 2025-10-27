using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchInventoryTransactionsEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransactionListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchInventoryTransactionsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchInventoryTransactionsEndpoint))
            .WithSummary("Gets a list of inventory transactions")
            .WithDescription("Gets a list of inventory transactions with pagination and filtering support")
            .Produces<PagedList<InventoryTransactionResponse>>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}

