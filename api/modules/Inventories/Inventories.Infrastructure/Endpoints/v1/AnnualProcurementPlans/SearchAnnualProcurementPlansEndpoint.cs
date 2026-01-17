using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AnnualProcurementPlans;

public static class SearchAnnualProcurementPlansEndpoint
{
    internal static RouteHandlerBuilder MapSearchAnnualProcurementPlansEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchAnnualProcurementPlansCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchAnnualProcurementPlansEndpoint))
            .WithSummary("search annual procurement plans (APP)")
            .WithDescription("search annual procurement plans with pagination")
            .Produces<PagedList<AnnualProcurementPlanListItemResponse>>()
            .RequirePermission("Permissions.AnnualProcurementPlans.Search")
            .MapToApiVersion(1);
    }
}

