using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.AssignInspector.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class AssignInspectorInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestAssignInspectorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/assign/{inspectorId:guid}", async (Guid id, Guid inspectorId, ISender mediator) =>
            {
                var response = await mediator.Send(new AssignInspectorToInspectionRequestCommand(id, inspectorId));
                return Results.Ok(response);
            })
            .WithName(nameof(AssignInspectorInspectionRequestEndpoint))
            .WithSummary("assign inspector to inspection request")
            .WithDescription("assign an inspector to an inspection request")
            .Produces<AssignInspectorToInspectionRequestResponse>()
            .RequirePermission("Permissions.InspectionRequests.Assign")
            .MapToApiVersion(1);
    }
}
