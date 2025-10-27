using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Approve.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
public static class ApproveInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new ApproveInspectionCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveInspectionEndpoint))
            .WithSummary("approve an inspection")
            .WithDescription("approve an inspection")
            .Produces<ApproveInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}
