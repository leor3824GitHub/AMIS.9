using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class RevertProcurementPlanToDraftEndpoint
{
    internal static RouteHandlerBuilder MapRevertProcurementPlanToDraftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/revert-to-draft", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new RevertProcurementPlanToDraftCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(RevertProcurementPlanToDraftEndpoint))
            .WithSummary("revert rejected procurement plan to draft")
            .WithDescription("reverts a rejected procurement plan back to draft status for re-editing")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.ProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}
