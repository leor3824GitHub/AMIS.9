using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Quarantine.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class QuarantineInspectionEndpoint
{
    internal static RouteHandlerBuilder MapQuarantineInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/quarantine", async (Guid id, QuarantineInspectionRequest request, ISender mediator) =>
            {
                var command = new QuarantineInspectionCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(QuarantineInspectionEndpoint))
            .WithSummary("Quarantine inspection")
            .WithDescription("Marks inspection as quarantined with a reason")
            .Produces<QuarantineInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}

public sealed record QuarantineInspectionRequest(string Reason);
