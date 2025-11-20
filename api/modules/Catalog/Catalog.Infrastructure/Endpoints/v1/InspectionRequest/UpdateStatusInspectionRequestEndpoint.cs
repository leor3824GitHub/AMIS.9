using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.UpdateStatus.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;

public static class UpdateStatusInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestUpdateStatusEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/status", async (Guid id, InspectionRequestStatus status, ISender mediator) =>
            {
                var response = await mediator.Send(new UpdateInspectionRequestStatusCommand(id, status));
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateStatusInspectionRequestEndpoint))
            .WithSummary("update inspection request status")
            .WithDescription("update the status of an inspection request")
            .Produces<UpdateInspectionRequestStatusResponse>()
            .RequirePermission("Permissions.InspectionRequests.StatusUpdate")
            .MapToApiVersion(1);
    }
}
