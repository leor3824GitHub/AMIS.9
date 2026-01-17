using AMIS.Inventories.Application.Issuances.Features.Accept.v1;
using AMIS.Framework.Infrastructure.Auth.Policy;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Inventories.Infrastructure.Issuances.Features.Accept.v1;

public static class AcceptIssuanceEndpoint
{
    public static RouteGroupBuilder MapAcceptIssuanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/accept", async (Guid id, [FromBody] AcceptRequest request, ISender mediator) =>
        {
            var command = new AcceptIssuanceCommand(id, request.SignedByEmployeeId);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(AcceptIssuanceEndpoint))
        .WithSummary("Accept an issuance with digital signature")
        .WithDescription("End user accepts asset custody with digital signature. Only pending issuances can be accepted.")
        .Produces<AcceptIssuanceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.Issuances.Accept")
        .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }

    public sealed record AcceptRequest(Guid SignedByEmployeeId);
}
