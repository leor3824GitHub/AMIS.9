using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteInventoryEndpoint
{
    internal static RouteHandlerBuilder MapInventoryDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteInventoryCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteInventoryEndpoint))
            .WithSummary("deletes inventory by id")
            .WithDescription("deletes inventory by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Inventories.Delete")
            .MapToApiVersion(1);
    }
}
