using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class MarkCompletedEndpoint
{
    internal static RouteHandlerBuilder MapMarkCompletedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-completed", async (Guid id, ISender mediator) =>
            {
                var command = new MarkCompletedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkCompletedEndpoint))
            .WithSummary("Mark inspection request as completed")
            .WithDescription("Transitions inspection request to Completed status")
            .RequirePermission("Permissions.InspectionRequests.Update")
            .Produces<MarkCompletedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
