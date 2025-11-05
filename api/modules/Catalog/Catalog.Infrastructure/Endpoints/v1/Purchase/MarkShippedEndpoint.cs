using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkShipped.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkShippedEndpoint
{
    internal static RouteHandlerBuilder MapMarkShippedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-shipped", async (Guid id, ISender mediator) =>
            {
                var command = new MarkShippedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkShippedEndpoint))
            .WithSummary("Mark purchase as shipped")
            .WithDescription("Changes purchase status to Shipped")
            .Produces<MarkShippedResponse>()
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
