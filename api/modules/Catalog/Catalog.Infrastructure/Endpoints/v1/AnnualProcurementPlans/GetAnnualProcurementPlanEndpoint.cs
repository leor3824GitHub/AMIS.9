using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class GetAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapGetAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetAnnualProcurementPlanRequest(id));
                return response is null ? Results.NotFound() : Results.Ok(response);
            })
            .WithName(nameof(GetAnnualProcurementPlanEndpoint))
            .WithSummary("get annual procurement plan")
            .Produces<GetAnnualProcurementPlanResponse>()
            .Produces(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.AnnualProcurementPlans.View")
            .MapToApiVersion(1);
    }
}
