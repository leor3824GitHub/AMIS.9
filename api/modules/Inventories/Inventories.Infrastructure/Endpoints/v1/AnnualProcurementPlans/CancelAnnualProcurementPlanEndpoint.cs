using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class CancelAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapCancelAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new CancelAnnualProcurementPlanCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(CancelAnnualProcurementPlanEndpoint))
            .WithSummary("cancel annual procurement plan")
            .WithDescription("cancels an annual procurement plan (not allowed for approved plans)")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}

