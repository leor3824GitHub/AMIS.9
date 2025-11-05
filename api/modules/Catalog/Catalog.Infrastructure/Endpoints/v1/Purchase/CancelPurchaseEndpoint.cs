using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class CancelPurchaseEndpoint
{
    internal static RouteHandlerBuilder MapCancelPurchaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, CancelRequest request, ISender mediator) =>
            {
                var command = new CancelPurchaseCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelPurchaseEndpoint))
            .WithSummary("Cancel purchase order")
            .WithDescription("Cancels a purchase order with a reason")
            .Produces<CancelPurchaseResponse>()
            .RequirePermission("Permissions.Purchases.Delete")
            .MapToApiVersion(1);
    }
}

public record CancelRequest(string Reason);
