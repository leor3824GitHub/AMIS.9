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

public static class SearchCategorysEndpoint
{
    internal static RouteHandlerBuilder MapGetCategoryListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchCategorysCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchCategorysEndpoint))
            .WithSummary("Gets a list of brands")
            .WithDescription("Gets a list of brands with pagination and filtering support")
            .Produces<PagedList<CategoryResponse>>()
            .RequirePermission("Permissions.Categorys.View")
            .MapToApiVersion(1);
    }
}
