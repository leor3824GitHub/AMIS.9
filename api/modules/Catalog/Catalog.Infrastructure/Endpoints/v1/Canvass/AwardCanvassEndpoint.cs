using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Award.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class AwardCanvassEndpoint
{
    internal static RouteHandlerBuilder MapAwardCanvassEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/award", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new AwardCanvassCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(AwardCanvassEndpoint))
            .WithSummary("Award canvass")
            .WithDescription("Marks the specified canvass as awarded (selected) and unselects all other canvasses for the same purchase request")
            .Produces<AwardCanvassResponse>()
            .RequirePermission("Permissions.Canvasses.Update")
            .MapToApiVersion(1);
    }
}
