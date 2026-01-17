using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class RejectAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapRejectAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (Guid id, RejectAnnualProcurementPlanBody body, ISender mediator) =>
            {
                await mediator.Send(new RejectAnnualProcurementPlanCommand(id, body.RejectedByUserId, body.Reason));
                return Results.NoContent();
            })
            .WithName(nameof(RejectAnnualProcurementPlanEndpoint))
            .WithSummary("reject annual procurement plan")
            .WithDescription("rejects a submitted annual procurement plan with a reason")
            .Accepts<RejectAnnualProcurementPlanBody>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.Approve")
            .MapToApiVersion(1);
    }
}

