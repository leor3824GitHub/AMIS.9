using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseItems.CreateBulk.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateBulkPurchaseItemEndpoint
{
    internal static RouteHandlerBuilder MapBulkPurchaseItemCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateBulkPurchaseItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateBulkPurchaseItemEndpoint))
            .WithSummary("create bulk purchaseItems")
            .WithDescription("create bulk purchaseItems")
            .Produces<CreateBulkPurchaseItemResponse>()
            .RequirePermission("Permissions.PurchaseItems.Create")
            .MapToApiVersion(1);
    }
}
