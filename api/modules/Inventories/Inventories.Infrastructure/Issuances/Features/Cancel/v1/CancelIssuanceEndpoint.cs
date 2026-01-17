using AMIS.Catalog.Application.Issuances.Features.Cancel.v1;
using AMIS.Framework.Infrastructure.Auth.Policy;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Catalog.Infrastructure.Issuances.Features.Cancel.v1;

public static class CancelIssuanceEndpoint
{
    public static RouteGroupBuilder MapCancelIssuanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
        {
            var command = new CancelIssuanceCommand(id);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(CancelIssuanceEndpoint))
        .WithSummary("Cancel an issuance")
        .WithDescription("Marks an issuance as cancelled. Cannot cancel returned or already cancelled issuances.")
        .Produces<CancelIssuanceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.Issuances.Cancel")
        .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
