using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Products.Get.v1;
using AMIS.WebApi.Catalog.Application.Products.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchProductsEndpoint
{
    internal static RouteHandlerBuilder MapGetProductListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchProductsCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchProductsEndpoint))
            .WithSummary("Gets a list of products")
            .WithDescription("Gets a list of products with pagination and filtering support")
            .Produces<PagedList<ProductResponse>>()
            .RequirePermission("Permissions.Products.View")
            .MapToApiVersion(1);
    }
}

