using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Products.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Product;
public static class CreateProductEndpoint
{
    internal static RouteHandlerBuilder MapProductCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProductCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateProductEndpoint))
            .WithSummary("creates a product")
            .WithDescription("creates a product")
            .Produces<CreateProductResponse>()
            .RequirePermission("Permissions.Products.Create")
            .MapToApiVersion(1);
    }
}

