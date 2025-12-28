using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class CreateProcurementPlanEndpoint
{
    internal static RouteHandlerBuilder MapProcurementPlanCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProcurementPlanCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateProcurementPlanEndpoint))
            .WithSummary("create procurement plan (PPMP)")
            .WithDescription("creates a PPMP header")
            .Produces<CreateProcurementPlanResponse>()
            .RequirePermission("Permissions.ProcurementPlans.Create")
            .MapToApiVersion(1);
    }
}
