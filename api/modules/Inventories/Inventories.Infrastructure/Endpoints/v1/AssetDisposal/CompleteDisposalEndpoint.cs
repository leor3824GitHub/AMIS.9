using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Disposals.Complete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetDisposal;

public static class CompleteDisposalEndpoint
{
    internal static RouteHandlerBuilder MapCompleteDisposalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/complete", async (Guid id, CompleteDisposalRequest request, ISender mediator) =>
            {
                var command = new CompleteDisposalCommand(id, request.SalvageValue, request.DisposalReferenceNumber);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CompleteDisposalEndpoint))
            .WithSummary("Complete disposal")
            .WithDescription("Complete an approved disposal. Records salvage value and calculates gain/loss.")
            .Produces<CompleteDisposalResponse>()
            .WithOpenApi()
            .RequirePermission("Permissions.Disposals.Complete")
            .MapToApiVersion(1);
    }
}

public record CompleteDisposalRequest(decimal? SalvageValue = null, string? DisposalReferenceNumber = null);
