using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.MarkFullyReceived.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class MarkFullyReceivedEndpoint
{
    internal static RouteHandlerBuilder MapMarkFullyReceivedEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/mark-fully-received", async (Guid id, ISender mediator) =>
            {
                var command = new MarkFullyReceivedCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(MarkFullyReceivedEndpoint))
            .WithSummary("Mark purchase as fully received")
            .WithDescription("Changes purchase status to FullyReceived after all items are accepted")
            .Produces<MarkFullyReceivedResponse>()
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
