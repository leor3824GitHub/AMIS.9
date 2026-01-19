using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public sealed class CancelPpeIssuanceReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-issuance")
            .WithTags("PPE Issuance");

        group.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
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
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
