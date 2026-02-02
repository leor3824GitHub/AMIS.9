using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Disposals.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetDisposal;

public static class CancelDisposalEndpoint
{
    internal static RouteHandlerBuilder MapCancelDisposalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cancel", async (Guid id, CancelDisposalRequest request, ISender mediator) =>
            {
                var command = new CancelDisposalCommand(id, request.CancellationReason);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelDisposalEndpoint))
            .WithSummary("Cancel disposal")
            .WithDescription("Cancel a pending or approved disposal request.")
            .Produces<CancelDisposalResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Disposals.Cancel")
            .MapToApiVersion(1);
    }
}

public record CancelDisposalRequest(string? CancellationReason = null);
