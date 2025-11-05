using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.MarkAsDamaged.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkAsDamagedEndpoint
{
    internal static RouteHandlerBuilder MapMarkAsDamagedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-damaged", async (Guid id, ISender mediator) =>
            {
                var command = new MarkAsDamagedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkAsDamagedEndpoint))
            .WithSummary("Mark inventory as damaged")
            .WithDescription("Marks inventory with damaged status")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<MarkAsDamagedResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
