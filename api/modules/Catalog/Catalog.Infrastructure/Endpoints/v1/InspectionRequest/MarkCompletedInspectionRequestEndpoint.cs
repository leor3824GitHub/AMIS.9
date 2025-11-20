using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class MarkCompletedInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestMarkCompletedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new MarkInspectionRequestCompletedCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(MarkCompletedInspectionRequestEndpoint))
            .WithSummary("mark inspection request completed")
            .WithDescription("mark an inspection request as completed")
            .Produces<MarkInspectionRequestCompletedResponse>()
            .RequirePermission("Permissions.InspectionRequests.Complete")
            .MapToApiVersion(1);
    }
}
