using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class CreateAnnualProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapAnnualProcurementPlanCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateAnnualProcurementPlanCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateAnnualProcurementPlanEndpoint))
            .WithSummary("create annual procurement plan (APP)")
            .WithDescription("creates an APP header")
            .Produces<CreateAnnualProcurementPlanResponse>()
            .RequirePermission("Permissions.AnnualProcurementPlans.Create")
            .MapToApiVersion(1);
    }
}
