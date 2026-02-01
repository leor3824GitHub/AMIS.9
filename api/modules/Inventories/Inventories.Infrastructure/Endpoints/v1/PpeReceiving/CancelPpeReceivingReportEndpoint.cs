using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class CancelPpeReceivingReportEndpoint
{
    public static RouteHandlerBuilder MapCancelPpeReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeReceiving.Cancel.v1.CancelPpeReceivingReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelPpeReceivingReportEndpoint))
            .WithSummary("Cancel a PPE Receiving Report")
            .WithDescription("Cancels a posted PPERR and creates reversal entries in the transaction log.")
            .Produces<Application.PpeReceiving.Cancel.v1.CancelPpeReceivingReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.PpeReceiving}.{FshActions.Cancel}")
            .MapToApiVersion(1);
    }
}
