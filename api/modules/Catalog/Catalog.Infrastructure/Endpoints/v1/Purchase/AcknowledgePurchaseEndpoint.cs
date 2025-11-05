using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Acknowledge.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class AcknowledgePurchaseEndpoint
{
    internal static RouteHandlerBuilder MapAcknowledgePurchaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/acknowledge", async (Guid id, ISender mediator) =>
            {
                var command = new AcknowledgePurchaseCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(AcknowledgePurchaseEndpoint))
            .WithSummary("Acknowledge an approved purchase")
            .WithDescription("Transitions purchase from Approved to Acknowledged status")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<AcknowledgePurchaseResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}
