using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public sealed class CancelPpeReceivingReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-receiving")
            .WithTags("PPE Receiving");

        group.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
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
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
