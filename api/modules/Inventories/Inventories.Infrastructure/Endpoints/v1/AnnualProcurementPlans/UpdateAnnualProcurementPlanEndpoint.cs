using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class UpdateAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAnnualProcurementPlanEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateAnnualProcurementPlanCommand request, ISender mediator) =>
            {
                request.Id = id;
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAnnualProcurementPlanEndpoint))
            .WithSummary("update annual procurement plan")
            .Produces<UpdateAnnualProcurementPlanResponse>()
            .RequirePermission("Permissions.AnnualProcurementPlans.Update")
            .MapToApiVersion(1);
    }
}

