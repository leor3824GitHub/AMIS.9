using AMIS.Catalog.Application.Issuances.Features.Reject.v1;
using AMIS.Framework.Infrastructure.Auth.Policy;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.Catalog.Infrastructure.Issuances.Features.Reject.v1;

public static class RejectIssuanceEndpoint
{
    public static RouteGroupBuilder MapRejectIssuanceEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/reject", async (Guid id, [FromBody] RejectRequest request, ISender mediator) =>
        {
            var command = new RejectIssuanceCommand(id, request.RejectionReason);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(RejectIssuanceEndpoint))
        .WithSummary("Reject an issuance with reason")
        .WithDescription("End user rejects asset custody with a rejection reason. Only pending issuances can be rejected.")
        .Produces<RejectIssuanceResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.Issuances.Reject")
        .MapToApiVersion(new ApiVersion(1, 0));

        return group;
    }

    public sealed record RejectRequest(string RejectionReason);
}
