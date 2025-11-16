using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CancelPurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseRequestCancelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, string? reason, ISender mediator) =>
            {
                await mediator.Send(new CancelPurchaseRequestCommand(id, reason));
                return Results.NoContent();
            })
            .WithName(nameof(CancelPurchaseRequestEndpoint))
            .WithSummary("cancel purchase request")
            .WithDescription("cancel a purchase request unless approved")
            .RequirePermission("Permissions.PurchaseRequests.Update")
            .MapToApiVersion(1);
    }
}
