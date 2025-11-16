using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetPurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapGetPurchaseRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPurchaseRequestCommand(id));
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName(nameof(GetPurchaseRequestEndpoint))
            .WithSummary("gets a purchase request")
            .WithDescription("gets a purchase request by id including items")
            .RequirePermission("Permissions.PurchaseRequests.View")
            .MapToApiVersion(1);
    }
}
