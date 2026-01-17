using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Acceptances.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Acceptance;

public static class CancelAcceptanceEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceCancelEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, string? reason, ISender mediator) =>
            {
                var response = await mediator.Send(new CancelAcceptanceCommand(id, reason));
                return Results.Ok(response);
            })
            .WithName(nameof(CancelAcceptanceEndpoint))
            .WithSummary("cancel an acceptance")
            .WithDescription("cancel an acceptance with optional reason")
            .Produces<CancelAcceptanceResponse>()
            .RequirePermission("Permissions.Acceptances.Cancel")
            .MapToApiVersion(1);
    }
}

