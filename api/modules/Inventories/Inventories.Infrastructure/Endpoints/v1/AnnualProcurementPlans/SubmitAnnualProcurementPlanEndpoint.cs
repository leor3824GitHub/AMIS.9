using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class SubmitAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapSubmitAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/submit", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new SubmitAnnualProcurementPlanCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(SubmitAnnualProcurementPlanEndpoint))
            .WithSummary("submit annual procurement plan")
            .WithDescription("submits a draft annual procurement plan for approval")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.Submit")
            .MapToApiVersion(1);
    }
}

