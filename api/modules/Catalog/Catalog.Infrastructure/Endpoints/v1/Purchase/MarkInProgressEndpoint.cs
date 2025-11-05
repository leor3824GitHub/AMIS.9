using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkInProgress.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkInProgressEndpoint
{
    internal static RouteHandlerBuilder MapMarkInProgressEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-in-progress", async (Guid id, ISender mediator) =>
            {
                var command = new MarkInProgressCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkInProgressEndpoint))
            .WithSummary("Mark purchase as in progress")
            .WithDescription("Transitions purchase from Acknowledged to InProgress status")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<MarkInProgressResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
