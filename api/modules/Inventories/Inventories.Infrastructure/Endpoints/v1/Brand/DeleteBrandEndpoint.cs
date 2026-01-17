using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Brands.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Brand;
public static class DeleteBrandEndpoint
{
    internal static RouteHandlerBuilder MapBrandDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteBrandCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteBrandEndpoint))
            .WithSummary("deletes brand by id")
            .WithDescription("deletes brand by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Brands.Delete")
            .MapToApiVersion(1);
    }
}

