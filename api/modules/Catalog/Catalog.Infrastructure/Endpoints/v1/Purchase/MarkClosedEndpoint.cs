using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkClosed.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkClosedEndpoint
{
    internal static RouteHandlerBuilder MapMarkClosedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-closed", async (Guid id, ISender mediator) =>
            {
                var command = new MarkClosedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkClosedEndpoint))
            .WithSummary("Mark purchase as closed")
            .WithDescription("Transitions invoiced/pending payment purchase to Closed status")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<MarkClosedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
