using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.ProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class RejectProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapRejectProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (Guid id, RejectProcurementPlanBody body, ISender mediator) =>
            {
                await mediator.Send(new RejectProcurementPlanCommand(id, body.RejectedByUserId, body.Reason));
                return Results.NoContent();
            })
            .WithName(nameof(RejectProcurementPlanEndpoint))
            .WithSummary("reject procurement plan")
            .WithDescription("rejects a submitted procurement plan with a reason")
            .Accepts<RejectProcurementPlanBody>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.ProcurementPlans.Approve")
            .MapToApiVersion(1);
    }
}

