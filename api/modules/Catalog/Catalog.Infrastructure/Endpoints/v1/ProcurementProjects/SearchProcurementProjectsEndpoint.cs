using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementProjects.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementProjects;

public static class SearchProcurementProjectsEndpoint
{
    internal static RouteHandlerBuilder MapSearchProcurementProjectsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchProcurementProjectsCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchProcurementProjectsEndpoint))
            .WithSummary("search procurement projects")
            .WithDescription("search procurement projects with pagination")
            .Produces<PagedList<ProcurementProjectListItemResponse>>()
            .RequirePermission("Permissions.ProcurementProjects.Search")
            .MapToApiVersion(1);
    }
}
