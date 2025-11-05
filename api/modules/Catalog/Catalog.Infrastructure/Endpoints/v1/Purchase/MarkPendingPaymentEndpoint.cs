using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkPendingPayment.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkPendingPaymentEndpoint
{
    internal static RouteHandlerBuilder MapMarkPendingPaymentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-pending-payment", async (Guid id, ISender mediator) =>
            {
                var command = new MarkPendingPaymentCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkPendingPaymentEndpoint))
            .WithSummary("Mark purchase as pending payment")
            .WithDescription("Transitions invoiced purchase to PendingPayment status")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<MarkPendingPaymentResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
