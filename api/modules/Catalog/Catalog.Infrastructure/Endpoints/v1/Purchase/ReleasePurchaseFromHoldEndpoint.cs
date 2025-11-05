using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.ReleaseFromHold.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ReleasePurchaseFromHoldEndpoint
{
    internal static RouteHandlerBuilder MapReleasePurchaseFromHoldEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/release-from-hold", async (Guid id, ISender mediator) =>
            {
                var command = new ReleasePurchaseFromHoldCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ReleasePurchaseFromHoldEndpoint))
            .WithSummary("Release purchase from hold")
            .WithDescription("Releases purchase from hold status back to Draft")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<ReleasePurchaseFromHoldResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
