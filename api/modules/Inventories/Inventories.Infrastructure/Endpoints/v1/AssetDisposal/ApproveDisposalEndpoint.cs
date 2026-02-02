using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Disposals.Approve.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetDisposal;

public static class ApproveDisposalEndpoint
{
    internal static RouteHandlerBuilder MapApproveDisposalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/approve", async (Guid id, ApproveDisposalRequest request, ISender mediator) =>
            {
                var command = new ApproveDisposalCommand(id, request.ApprovalNotes);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveDisposalEndpoint))
            .WithSummary("Approve disposal request")
            .WithDescription("Approve a pending disposal request. Requires Disposal.Approve permission.")
            .Produces<ApproveDisposalResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Disposals.Approve")
            .MapToApiVersion(1);
    }
}

public record ApproveDisposalRequest(string? ApprovalNotes = null);
