using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.PutOnHold.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class PutInspectionOnHoldEndpoint
{
    internal static RouteHandlerBuilder MapPutInspectionOnHoldEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/put-on-hold", async (Guid id, InspectionHoldRequest request, ISender mediator) =>
            {
                var command = new PutInspectionOnHoldCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PutInspectionOnHoldEndpoint))
            .WithSummary("Put inspection on hold")
            .WithDescription("Places inspection on hold with specified reason")
            .RequirePermission("Permissions.Inspections.Update")
            .Produces<PutInspectionOnHoldResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record InspectionHoldRequest(string Reason);
