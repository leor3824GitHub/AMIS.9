using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Categories.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteCategoryEndpoint
{
    internal static RouteHandlerBuilder MapCategoryDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteCategoryCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteCategoryEndpoint))
            .WithSummary("deletes brand by id")
            .WithDescription("deletes brand by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Categories.Delete")
            .MapToApiVersion(1);
    }
}
