using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapInventoryTransactionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInventoryTransactionCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInventoryTransactionEndpoint))
            .WithSummary("update a product inventory transaction")
            .WithDescription("update a product inventory transaction")
            .Produces<UpdateInventoryTransactionResponse>()
            .RequirePermission("Permissions.InventoryTransactions.Update")
            .MapToApiVersion(1);
    }
}
