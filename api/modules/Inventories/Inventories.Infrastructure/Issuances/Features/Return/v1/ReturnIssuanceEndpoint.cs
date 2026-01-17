using AMIS.Catalog.Application.Issuances.Features.Return.v1;
using AMIS.Framework.Infrastructure.Auth.Policy;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Catalog.Infrastructure.Issuances.Features.Return.v1;

public static class ReturnIssuanceEndpoint
{
    public static RouteGroupBuilder MapReturnIssuanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/return", async (Guid id, ISender mediator) =>
        {
            var command = new ReturnIssuanceCommand(id);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(ReturnIssuanceEndpoint))
        .WithSummary("Mark issuance as returned")
        .WithDescription("Marks an accepted issuance as returned. Only accepted issuances can be marked as returned.")
        .Produces<ReturnIssuanceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.Issuances.Return")
        .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }
}
