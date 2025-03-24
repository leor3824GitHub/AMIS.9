using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateInventoryEndpoint
{
    internal static RouteHandlerBuilder MapInventoryUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInventoryCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInventoryEndpoint))
            .WithSummary("update a inventory")
            .WithDescription("update a inventory")
            .Produces<UpdateInventoryResponse>()
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(1);
    }
}
