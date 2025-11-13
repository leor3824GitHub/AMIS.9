using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Reject.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
public static class RejectInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (Guid id, string? reason, ISender mediator) =>
            {
                var response = await mediator.Send(new RejectInspectionCommand(id, reason));
                return Results.Ok(response);
            })
            .WithName(nameof(RejectInspectionEndpoint))
            .WithSummary("reject an inspection")
            .WithDescription("reject an inspection with optional reason")
            .Produces<RejectInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}
