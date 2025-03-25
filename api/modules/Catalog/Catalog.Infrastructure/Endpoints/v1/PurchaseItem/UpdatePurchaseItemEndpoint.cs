using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdatePurchaseItemEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdatePurchaseItemCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePurchaseItemEndpoint))
            .WithSummary("update a purchaseItem")
            .WithDescription("update a purchaseItem")
            .Produces<UpdatePurchaseItemResponse>()
            .RequirePermission("Permissions.PurchaseItems.Update")
            .MapToApiVersion(1);
    }
}
