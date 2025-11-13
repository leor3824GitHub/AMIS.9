using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
public static class CancelInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionCancelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, string? reason, ISender mediator) =>
            {
                var response = await mediator.Send(new CancelInspectionCommand(id, reason));
                return Results.Ok(response);
            })
            .WithName(nameof(CancelInspectionEndpoint))
            .WithSummary("cancel an inspection")
            .WithDescription("cancel an inspection with optional reason")
            .Produces<CancelInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}
