using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.MarkAsObsolete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkAsObsoleteEndpoint
{
    internal static RouteHandlerBuilder MapMarkAsObsoleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-obsolete", async (Guid id, ISender mediator) =>
            {
                var command = new MarkAsObsoleteCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkAsObsoleteEndpoint))
            .WithSummary("Mark inventory as obsolete")
            .WithDescription("Marks inventory with obsolete status")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<MarkAsObsoleteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
