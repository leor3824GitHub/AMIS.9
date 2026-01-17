using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.ProcurementPlans.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class UpdateProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapUpdateProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateProcurementPlanCommand request, ISender mediator) =>
            {
                request.Id = id;
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateProcurementPlanEndpoint))
            .WithSummary("update procurement plan (PPMP)")
            .WithDescription("updates a draft PPMP header")
            .Produces<UpdateProcurementPlanResponse>()
            .RequirePermission("Permissions.ProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}

