using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;

public static class CompleteInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionCompleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new CompleteInspectionCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(CompleteInspectionEndpoint))
            .WithSummary("complete an inspection")
            .WithDescription("mark an inspection as completed")
            .Produces<CompleteInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Complete")
            .MapToApiVersion(1);
    }
}
