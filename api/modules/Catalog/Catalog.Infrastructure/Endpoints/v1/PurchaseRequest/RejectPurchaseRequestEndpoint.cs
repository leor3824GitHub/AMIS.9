using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class RejectPurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseRequestRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (Guid id, Guid rejectedBy, string reason, ISender mediator) =>
            {
                await mediator.Send(new RejectPurchaseRequestCommand(id, rejectedBy, reason));
                return Results.NoContent();
            })
            .WithName(nameof(RejectPurchaseRequestEndpoint))
            .WithSummary("reject purchase request")
            .WithDescription("reject a submitted purchase request")
            .RequirePermission("Permissions.PurchaseRequests.Update")
            .MapToApiVersion(1);
    }
}
