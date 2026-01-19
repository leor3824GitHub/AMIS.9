using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InventoryTransactionLogs.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryTransactionLog;

public static class GetInventoryTransactionLogEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransactionLogEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInventoryTransactionLogRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryTransactionLogEndpoint))
            .WithSummary("Gets an inventory transaction log entry by id")
            .WithDescription("Gets an inventory transaction log entry by id")
            .Produces<InventoryTransactionLogResponse>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
