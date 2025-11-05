using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Reject.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class RejectPurchaseEndpoint
{
    internal static RouteHandlerBuilder MapRejectPurchaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/reject", async (Guid id, RejectRequest request, ISender mediator) =>
            {
                var command = new RejectPurchaseCommand(id, request.Reason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectPurchaseEndpoint))
            .WithSummary("Reject purchase")
            .WithDescription("Changes purchase status from PendingApproval to Rejected with a reason")
            .Produces<RejectPurchaseResponse>()
            .RequirePermission("Permissions.Purchases.Approve")
            .MapToApiVersion(1);
    }
}

public sealed record RejectRequest(string Reason);
