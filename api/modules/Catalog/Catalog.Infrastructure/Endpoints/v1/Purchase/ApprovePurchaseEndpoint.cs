using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Approve.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ApprovePurchaseEndpoint
{
    internal static RouteHandlerBuilder MapApprovePurchaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (Guid id, ISender mediator) =>
            {
                var command = new ApprovePurchaseCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ApprovePurchaseEndpoint))
            .WithSummary("Approve purchase")
            .WithDescription("Changes purchase status from PendingApproval to Approved")
            .Produces<ApprovePurchaseResponse>()
            .RequirePermission("Permissions.Purchases.Approve")
            .MapToApiVersion(1);
    }
}
