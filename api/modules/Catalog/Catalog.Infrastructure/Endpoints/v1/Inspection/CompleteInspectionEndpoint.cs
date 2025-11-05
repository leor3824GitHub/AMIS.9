using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class CompleteInspectionEndpoint
{
    internal static RouteHandlerBuilder MapCompleteInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (Guid id, ISender mediator) =>
            {
                var command = new CompleteInspectionCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CompleteInspectionEndpoint))
            .WithSummary("Complete inspection")
            .WithDescription("Finalizes inspection and transitions to Completed status")
            .RequirePermission("Permissions.Inspections.Update")
            .Produces<CompleteInspectionResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
