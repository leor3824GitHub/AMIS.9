using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Inspections.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Inspection
{
    public static class DeleteInspectionEndpoint
    {
        internal static RouteHandlerBuilder MapInspectionDeletionEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var command = new DeleteInspectionCommand(id);
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(DeleteInspectionEndpoint))
                .WithSummary("Deletes an inspection")
                .WithDescription("Deletes an inspection by Id")
                .Produces<DeleteInspectionResponse>()
                .RequirePermission("Permissions.Inspections.Delete")
                .MapToApiVersion(1);
        }
    }
}

