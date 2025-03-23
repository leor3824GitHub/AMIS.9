using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Categories.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCategoryCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCategoryCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateCategoryEndpoint))
            .WithSummary("creates a category")
            .WithDescription("creates a category")
            .Produces<CreateCategoryResponse>()
            .RequirePermission("Permissions.Categorys.Create")
            .MapToApiVersion(1);
    }
}
