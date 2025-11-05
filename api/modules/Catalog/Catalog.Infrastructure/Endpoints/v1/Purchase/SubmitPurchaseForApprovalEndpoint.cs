using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.SubmitForApproval.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SubmitPurchaseForApprovalEndpoint
{
    internal static RouteHandlerBuilder MapSubmitPurchaseForApprovalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/submit-for-approval", async (Guid id, ISender mediator) =>
            {
                var command = new SubmitPurchaseForApprovalCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SubmitPurchaseForApprovalEndpoint))
            .WithSummary("Submit purchase for approval")
            .WithDescription("Changes purchase status from Draft to PendingApproval")
            .Produces<SubmitPurchaseForApprovalResponse>()
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
