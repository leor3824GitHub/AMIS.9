using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetInventoryEndpoint
{
    internal static RouteHandlerBuilder MapGetInventoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInventoryRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInventoryEndpoint))
            .WithSummary("gets inventory by id")
            .WithDescription("gets prodct by id")
            .Produces<InventoryResponse>()
            .RequirePermission("Permissions.Inventories.View")
            .MapToApiVersion(1);
    }
}
