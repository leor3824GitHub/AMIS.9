using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkInvoiced.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkInvoicedEndpoint
{
    internal static RouteHandlerBuilder MapMarkInvoicedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-invoiced", async (Guid id, ISender mediator) =>
            {
                var command = new MarkInvoicedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkInvoicedEndpoint))
            .WithSummary("Mark purchase as invoiced")
            .WithDescription("Changes purchase status to Invoiced")
            .Produces<MarkInvoicedResponse>()
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
