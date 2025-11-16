using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class ApprovePurchaseRequestEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseRequestApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (Guid id, Guid approvedBy, string? remarks, ISender mediator) =>
            {
                await mediator.Send(new ApprovePurchaseRequestCommand(id, approvedBy, remarks));
                return Results.NoContent();
            })
            .WithName(nameof(ApprovePurchaseRequestEndpoint))
            .WithSummary("approve purchase request")
            .WithDescription("approve a submitted purchase request")
            .RequirePermission("Permissions.PurchaseRequests.Update")
            .MapToApiVersion(1);
    }
}
