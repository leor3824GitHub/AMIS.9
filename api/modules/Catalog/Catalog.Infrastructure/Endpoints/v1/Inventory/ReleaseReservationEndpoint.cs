using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.ReleaseReservation.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReleaseReservationEndpoint
{
    internal static RouteHandlerBuilder MapReleaseReservationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/release-reservation", async (Guid id, ReleaseRequest request, ISender mediator) =>
            {
                var command = new ReleaseReservationCommand(id, request.Quantity);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleaseReservationEndpoint))
            .WithSummary("Release reserved inventory stock")
            .WithDescription("Releases a specified quantity from reserved stock back to available")
            .Produces<ReleaseReservationResponse>()
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(1);
    }
}

public record ReleaseRequest(int Quantity);
