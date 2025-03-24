using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateInventoryEndpoint
{
    internal static RouteHandlerBuilder MapInventoryCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInventoryCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInventoryEndpoint))
            .WithSummary("creates a inventory")
            .WithDescription("creates a inventory")
            .Produces<CreateInventoryResponse>()
            .RequirePermission("Permissions.Inventories.Create")
            .MapToApiVersion(1);
    }
}
