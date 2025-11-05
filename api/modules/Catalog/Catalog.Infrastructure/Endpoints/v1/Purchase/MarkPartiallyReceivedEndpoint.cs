using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkPartiallyReceived.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkPartiallyReceivedEndpoint
{
    internal static RouteHandlerBuilder MapMarkPartiallyReceivedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-partially-received", async (Guid id, ISender mediator) =>
            {
                var command = new MarkPartiallyReceivedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkPartiallyReceivedEndpoint))
            .WithSummary("Mark purchase as partially received")
            .WithDescription("Transitions purchase to PartiallyReceived status when some items are received")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<MarkPartiallyReceivedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
