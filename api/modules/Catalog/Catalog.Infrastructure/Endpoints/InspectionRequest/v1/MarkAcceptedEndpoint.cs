using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class MarkAcceptedEndpoint
{
    internal static RouteHandlerBuilder MapMarkAcceptedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-accepted", async (Guid id, ISender mediator) =>
            {
                var command = new MarkAcceptedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkAcceptedEndpoint))
            .WithSummary("Mark inspection request as accepted")
            .WithDescription("Transitions inspection request to Accepted status")
            .RequirePermission("Permissions.InspectionRequests.Update")
            .Produces<MarkAcceptedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
