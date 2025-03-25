using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetPurchaseEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPurchaseRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetPurchaseEndpoint))
            .WithSummary("gets purchase by id")
            .WithDescription("gets purchase by id")
            .Produces<PurchaseResponse>()
            .RequirePermission("Permissions.Purchases.View")
            .MapToApiVersion(1);
    }
}
