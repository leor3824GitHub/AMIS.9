using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class RevertAnnualProcurementPlanToDraftEndpoint
{
    internal static RouteHandlerBuilder MapRevertAnnualProcurementPlanToDraftEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/revert-to-draft", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new RevertAnnualProcurementPlanToDraftCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(RevertAnnualProcurementPlanToDraftEndpoint))
            .WithSummary("revert rejected annual procurement plan to draft")
            .WithDescription("reverts a rejected annual procurement plan back to draft status for re-editing")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}

