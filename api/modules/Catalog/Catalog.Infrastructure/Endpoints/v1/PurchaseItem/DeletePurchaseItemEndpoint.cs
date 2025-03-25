using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeletePurchaseItemEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseItemDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeletePurchaseItemCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeletePurchaseItemEndpoint))
            .WithSummary("deletes purchaseItem by id")
            .WithDescription("deletes purchaseItem by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.PurchaseItems.Delete")
            .MapToApiVersion(1);
    }
}
