using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public sealed class DeletePpeReceivingReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-receiving")
            .WithTags("PPE Receiving");

        group.MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeReceiving.Delete.v1.DeletePpeReceivingReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePpeReceivingReportEndpoint))
            .WithSummary("Delete a PPE Receiving Report")
            .WithDescription("Deletes a draft PPERR. Only draft reports can be deleted.")
            .Produces<Application.PpeReceiving.Delete.v1.DeletePpeReceivingReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
