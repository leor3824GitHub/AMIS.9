using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

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
