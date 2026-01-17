using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.ProcurementPlans.Items.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class ProcurementPlanItemsEndpoints
{
    internal static IEndpointRouteBuilder MapProcurementPlanItemsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{planId:guid}/items", async (Guid planId, AddProcurementPlanItemCommand request, ISender mediator) =>
            {
                request.PlanId = planId;
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName("AddProcurementPlanItem")
            .WithSummary("add PPMP item")
            .RequirePermission("Permissions.ProcurementPlanItems.Create")
            .MapToApiVersion(1);

        endpoints.MapPut("/{planId:guid}/items/{itemId:guid}", async (Guid planId, Guid itemId, UpdateProcurementPlanItemCommand request, ISender mediator) =>
            {
                request.PlanId = planId;
                request.ItemId = itemId;
                await mediator.Send(request);
                return Results.NoContent();
            })
            .WithName("UpdateProcurementPlanItem")
            .WithSummary("update PPMP item")
            .RequirePermission("Permissions.ProcurementPlanItems.Update")
            .MapToApiVersion(1);

        endpoints.MapDelete("/{planId:guid}/items/{itemId:guid}", async (Guid planId, Guid itemId, ISender mediator) =>
            {
                await mediator.Send(new DeleteProcurementPlanItemCommand(planId, itemId));
                return Results.NoContent();
            })
            .WithName("DeleteProcurementPlanItem")
            .WithSummary("delete PPMP item")
            .RequirePermission("Permissions.ProcurementPlanItems.Delete")
            .MapToApiVersion(1);

        return endpoints;
    }
}

