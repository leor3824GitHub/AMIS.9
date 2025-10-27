using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapInventoryTransactionDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteInventoryTransactionCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteInventoryTransactionEndpoint))
            .WithSummary("deletes product inventory by id")
            .WithDescription("deletes product inventory by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.InventoryTransactions.Delete")
            .MapToApiVersion(1);
    }
}
