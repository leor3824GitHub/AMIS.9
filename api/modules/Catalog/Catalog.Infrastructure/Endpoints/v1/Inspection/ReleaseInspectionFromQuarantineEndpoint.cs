using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromQuarantine.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReleaseInspectionFromQuarantineEndpoint
{
    internal static RouteHandlerBuilder MapReleaseInspectionFromQuarantineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/release-from-quarantine", async (Guid id, ISender mediator) =>
            {
                var command = new ReleaseInspectionFromQuarantineCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseInspectionFromQuarantineEndpoint))
            .WithSummary("Release inspection from quarantine")
            .WithDescription("Releases inspection from quarantine status")
            .RequirePermission("Permissions.Inspections.Update")
            .Produces<ReleaseInspectionFromQuarantineResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
