using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.SetLocation.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SetLocationEndpoint
{
    internal static RouteHandlerBuilder MapSetLocationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/set-location", async (Guid id, LocationRequest request, ISender mediator) =>
            {
                var command = new SetLocationCommand(id, request.Location);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SetLocationEndpoint))
            .WithSummary("Set inventory location")
            .WithDescription("Updates the storage location for inventory")
            .RequirePermission("Permissions.Inventories.Update")
            .Produces<SetLocationResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record LocationRequest(string Location);
