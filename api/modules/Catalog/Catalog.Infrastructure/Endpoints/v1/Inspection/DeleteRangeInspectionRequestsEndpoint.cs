using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.DeleteRange.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1
{
    public static class DeleteRangeInspectionsEndpoint
    {
        internal static RouteHandlerBuilder MapInspectionDeletionRangeEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/", async ([FromBody] List<Guid> inspectionrequestIds, ISender mediator) =>
                {
                    if (inspectionrequestIds == null || inspectionrequestIds.Count == 0)
                    {
                        return Results.BadRequest("Ids for Inspection request cannot be null or empty.");
                    }

                    try
                    {
                        await mediator.Send(new DeleteRangeInspectionsCommand(inspectionrequestIds));
                        return Results.NoContent();
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                })
                .WithName(nameof(DeleteRangeInspectionsEndpoint))
                .WithSummary("Deletes inspections by IDs")
                .WithDescription("Deletes inspections by IDs")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequirePermission("Permissions.Inspections.Delete")
                .MapToApiVersion(1);
        }
    }
}
