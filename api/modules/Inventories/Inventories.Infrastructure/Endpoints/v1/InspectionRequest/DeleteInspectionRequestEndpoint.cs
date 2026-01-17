using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.InspectionRequests.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InspectionRequest
{
    public static class DeleteInspectionRequestEndpoint
    {
        internal static RouteHandlerBuilder MapInspectionRequestDeletionEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var command = new DeleteInspectionRequestCommand(id);
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(DeleteInspectionRequestEndpoint))
                .WithSummary("Deletes an inspectionRequest")
                .WithDescription("Deletes an inspectionRequest by Id")
                .Produces<DeleteInspectionRequestResponse>()
                .RequirePermission("Permissions.InspectionRequests.Delete")
                .MapToApiVersion(1);
        }
    }
}

