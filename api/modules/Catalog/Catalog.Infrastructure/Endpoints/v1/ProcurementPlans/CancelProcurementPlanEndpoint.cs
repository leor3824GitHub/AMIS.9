using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class CancelProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapCancelProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
            {
                await mediator.Send(new CancelProcurementPlanCommand(id));
                return Results.NoContent();
            })
            .WithName(nameof(CancelProcurementPlanEndpoint))
            .WithSummary("cancel procurement plan")
            .WithDescription("cancels a procurement plan (not allowed for approved plans)")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.ProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}
