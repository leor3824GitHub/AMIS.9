using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

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
