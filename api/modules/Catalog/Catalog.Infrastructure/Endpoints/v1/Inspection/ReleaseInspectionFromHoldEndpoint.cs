using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromHold.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReleaseInspectionFromHoldEndpoint
{
    internal static RouteHandlerBuilder MapReleaseInspectionFromHoldEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/release-from-hold", async (Guid id, ISender mediator) =>
            {
                var command = new ReleaseInspectionFromHoldCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseInspectionFromHoldEndpoint))
            .WithSummary("Release inspection from hold")
            .WithDescription("Releases inspection from hold status back to InProgress")
            .RequirePermission("Permissions.Inspections.Update")
            .Produces<ReleaseInspectionFromHoldResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
