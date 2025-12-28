using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Items.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class AnnualProcurementPlanItemsEndpoints
{
    internal static IEndpointRouteBuilder MapAnnualProcurementPlanItemsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{planId:guid}/items", async (Guid planId, AddAnnualProcurementPlanItemCommand request, ISender mediator) =>
            {
                request.PlanId = planId;
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName("AddAnnualProcurementPlanItem")
            .WithSummary("add APP item")
            .RequirePermission("Permissions.AnnualProcurementPlanItems.Create")
            .MapToApiVersion(1);

        endpoints.MapPut("/{planId:guid}/items/{itemId:guid}", async (Guid planId, Guid itemId, UpdateAnnualProcurementPlanItemCommand request, ISender mediator) =>
            {
                request.PlanId = planId;
                request.ItemId = itemId;
                await mediator.Send(request);
                return Results.NoContent();
            })
            .WithName("UpdateAnnualProcurementPlanItem")
            .WithSummary("update APP item")
            .RequirePermission("Permissions.AnnualProcurementPlanItems.Update")
            .MapToApiVersion(1);

        endpoints.MapDelete("/{planId:guid}/items/{itemId:guid}", async (Guid planId, Guid itemId, ISender mediator) =>
            {
                await mediator.Send(new DeleteAnnualProcurementPlanItemCommand(planId, itemId));
                return Results.NoContent();
            })
            .WithName("DeleteAnnualProcurementPlanItem")
            .WithSummary("delete APP item")
            .RequirePermission("Permissions.AnnualProcurementPlanItems.Delete")
            .MapToApiVersion(1);

        return endpoints;
    }
}
