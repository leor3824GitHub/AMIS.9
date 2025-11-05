using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkPendingInvoice.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkPendingInvoiceEndpoint
{
    internal static RouteHandlerBuilder MapMarkPendingInvoiceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-pending-invoice", async (Guid id, ISender mediator) =>
            {
                var command = new MarkPendingInvoiceCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkPendingInvoiceEndpoint))
            .WithSummary("Mark purchase as pending invoice")
            .WithDescription("Transitions received purchase to PendingInvoice status")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<MarkPendingInvoiceResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
