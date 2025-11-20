using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class MarkAcceptedInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestMarkAcceptedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/accept", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new MarkInspectionRequestAcceptedCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(MarkAcceptedInspectionRequestEndpoint))
            .WithSummary("mark inspection request accepted")
            .WithDescription("mark an inspection request as accepted")
            .Produces<MarkInspectionRequestAcceptedResponse>()
            .RequirePermission("Permissions.InspectionRequests.Accept")
            .MapToApiVersion(1);
    }
}
