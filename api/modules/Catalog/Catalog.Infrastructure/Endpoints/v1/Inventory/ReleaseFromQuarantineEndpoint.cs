using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.ReleaseFromQuarantine.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReleaseFromQuarantineEndpoint
{
    internal static RouteHandlerBuilder MapReleaseFromQuarantineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/release-from-quarantine", async (Guid id, ISender mediator) =>
            {
                var command = new ReleaseFromQuarantineCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseFromQuarantineEndpoint))
            .WithSummary("Release inventory from quarantine")
            .WithDescription("Releases quarantined inventory and marks as available")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<ReleaseFromQuarantineResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
