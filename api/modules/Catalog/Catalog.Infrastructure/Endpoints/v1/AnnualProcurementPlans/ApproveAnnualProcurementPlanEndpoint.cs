using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class ApproveAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapApproveAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (Guid id, ApproveAnnualProcurementPlanBody body, ISender mediator) =>
            {
                await mediator.Send(new ApproveAnnualProcurementPlanCommand(id, body.ApprovedByUserId));
                return Results.NoContent();
            })
            .WithName(nameof(ApproveAnnualProcurementPlanEndpoint))
            .WithSummary("approve annual procurement plan")
            .WithDescription("approves a submitted annual procurement plan")
            .Accepts<ApproveAnnualProcurementPlanBody>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.Approve")
            .MapToApiVersion(1);
    }
}
