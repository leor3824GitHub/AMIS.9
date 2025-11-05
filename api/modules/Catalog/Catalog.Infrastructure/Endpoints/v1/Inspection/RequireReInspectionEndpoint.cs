using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.RequireReInspection.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class RequireReInspectionEndpoint
{
    internal static RouteHandlerBuilder MapRequireReInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/require-reinspection", async (Guid id, ReInspectionRequest request, ISender mediator) =>
            {
                var command = new RequireReInspectionCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(RequireReInspectionEndpoint))
            .WithSummary("Require re-inspection")
            .WithDescription("Marks inspection as requiring re-inspection with specified reason")
            .RequirePermission("Permissions.Inspections.Update")
            .Produces<RequireReInspectionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record ReInspectionRequest(string Reason);
