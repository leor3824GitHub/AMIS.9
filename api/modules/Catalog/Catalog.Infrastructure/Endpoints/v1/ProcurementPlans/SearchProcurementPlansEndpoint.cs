using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementPlans;

public static class SearchProcurementPlansEndpoint
{
    internal static RouteHandlerBuilder MapSearchProcurementPlansEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchProcurementPlansCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchProcurementPlansEndpoint))
            .WithSummary("search procurement plans (PPMP)")
            .WithDescription("search PPMP with pagination")
            .Produces<PagedList<ProcurementPlanListItemResponse>>()
            .RequirePermission("Permissions.ProcurementPlans.Search")
            .MapToApiVersion(1);
    }
}
