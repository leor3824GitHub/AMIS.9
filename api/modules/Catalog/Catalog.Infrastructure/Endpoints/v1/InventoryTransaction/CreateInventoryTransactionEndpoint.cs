using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InventoryTransactions.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateInventoryTransactionEndpoint
{
    internal static RouteHandlerBuilder MapInventoryTransactionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInventoryTransactionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInventoryTransactionEndpoint))
            .WithSummary("creates a inventory for product")
            .WithDescription("creates a inventory for product")
            .Produces<CreateInventoryTransactionResponse>()
            .RequirePermission("Permissions.InventoryTransactions.Create")
            .MapToApiVersion(1);
    }
}
