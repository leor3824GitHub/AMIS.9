using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class GetProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapGetProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProcurementPlanRequest(id));
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName(nameof(GetProcurementPlanEndpoint))
            .WithSummary("get procurement plan (PPMP)")
            .WithDescription("gets a PPMP by id")
            .Produces<GetProcurementPlanResponse>()
            .RequirePermission("Permissions.ProcurementPlans.View")
            .MapToApiVersion(1);
    }
}
