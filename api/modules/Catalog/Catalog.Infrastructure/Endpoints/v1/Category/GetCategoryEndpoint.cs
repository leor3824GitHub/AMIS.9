using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Categories.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetCategoryEndpoint
{
    internal static RouteHandlerBuilder MapGetCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCategoryRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetCategoryEndpoint))
            .WithSummary("gets brand by id")
            .WithDescription("gets brand by id")
            .Produces<CategoryResponse>()
            .RequirePermission("Permissions.Categories.View")
            .MapToApiVersion(1);
    }
}
