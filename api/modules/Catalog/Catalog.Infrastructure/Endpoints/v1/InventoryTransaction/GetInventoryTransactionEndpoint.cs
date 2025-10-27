using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInventoryTransactionRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryTransactionEndpoint))
            .WithSummary("gets product inventory transaction by id")
            .WithDescription("gets product inventory transaction by id")
            .Produces<InventoryTransactionResponse>()
            .RequirePermission("Permissions.InventoryTransactions.View")
            .MapToApiVersion(1);
    }
}
