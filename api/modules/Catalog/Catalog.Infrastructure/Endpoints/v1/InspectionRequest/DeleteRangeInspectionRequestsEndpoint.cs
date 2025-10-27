using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.DeleteRange.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1
{
    public static class DeleteRangeInspectionRequestsEndpoint
    {
        internal static RouteHandlerBuilder MapInspectionRequestDeletionRangeEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/", async ([FromBody] List<Guid> inspectionrequestIds, ISender mediator) =>
                {
                    if (inspectionrequestIds == null || inspectionrequestIds.Count == 0)
                    {
                        return Results.BadRequest("Ids for inspection request cannot be null or empty.");
                    }

                    try
                    {
                        await mediator.Send(new DeleteRangeInspectionRequestsCommand(inspectionrequestIds));
                        return Results.NoContent();
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                })
                .WithName(nameof(DeleteRangeInspectionRequestsEndpoint))
                .WithSummary("Deletes inspection requests by IDs")
                .WithDescription("Deletes inspection request by IDs")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequirePermission("Permissions.InspectionRequests.Delete")
                .MapToApiVersion(1);
        }
    }
}
