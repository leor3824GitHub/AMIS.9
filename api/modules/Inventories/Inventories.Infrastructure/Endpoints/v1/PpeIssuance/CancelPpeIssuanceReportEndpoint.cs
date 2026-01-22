using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class CancelPpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapCancelPpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeIssuance.Cancel.v1.CancelPpeIssuanceReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelPpeIssuanceReportEndpoint))
            .WithSummary("Cancel a PPE Issuance Report")
            .WithDescription("Cancels a posted PPEIR, reverses inventory deductions, and creates reversal entries in the transaction log.")
            .Produces<Application.PpeIssuance.Cancel.v1.CancelPpeIssuanceReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.PpeIssuance.Cancel")
            .MapToApiVersion(1);
    }
}
