using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using AMIS.WebApi.Catalog.Application.Categories.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchCategoriesEndpoint
{
    internal static RouteHandlerBuilder MapSearchCategoriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCategorysCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCategoriesEndpoint))
            .WithSummary("Search categories")
            .WithDescription("Search categories with pagination and filtering support")
            .Produces<PagedList<CategoryResponse>>()
            .RequirePermission("Permissions.Categories.View")
            .MapToApiVersion(1);
    }
}
