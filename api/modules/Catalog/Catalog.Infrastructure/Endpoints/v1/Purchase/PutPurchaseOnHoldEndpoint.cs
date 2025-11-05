using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.PutOnHold.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class PutPurchaseOnHoldEndpoint
{
    internal static RouteHandlerBuilder MapPutPurchaseOnHoldEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/put-on-hold", async (Guid id, HoldRequest request, ISender mediator) =>
            {
                var command = new PutPurchaseOnHoldCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PutPurchaseOnHoldEndpoint))
            .WithSummary("Put purchase on hold")
            .WithDescription("Places purchase on hold with specified reason")
            .RequirePermission("Permissions.Purchases.Update")
            .Produces<PutPurchaseOnHoldResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record HoldRequest(string Reason);
