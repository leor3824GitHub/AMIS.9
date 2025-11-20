using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Get.v1;
using AMIS.WebApi.Catalog.Application.Canvasses.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class SearchCanvassesEndpoint
{
    internal static RouteHandlerBuilder MapSearchCanvassesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchCanvassesCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCanvassesEndpoint))
            .WithSummary("Search canvasses")
            .WithDescription("Searches and retrieves a paginated list of canvasses with optional filters")
            .Produces<PagedList<CanvassResponse>>()
            .RequirePermission("Permissions.Canvasses.View")
            .MapToApiVersion(1);
    }
}
