using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class SubmitProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapSubmitProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/submit", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new SubmitProcurementPlanCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(SubmitProcurementPlanEndpoint))
            .WithSummary("submit procurement plan (PPMP)")
            .WithDescription("submits a draft PPMP for approval")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.ProcurementPlans.Submit")
            .MapToApiVersion(1);
    }
}
