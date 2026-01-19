using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public sealed class UpdatePpeReceivingReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-receiving")
            .WithTags("PPE Receiving");

        group.MapPut("/{id:guid}", async (Guid id, Application.PpeReceiving.Update.v1.UpdatePpeReceivingReportCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePpeReceivingReportEndpoint))
            .WithSummary("Update a PPE Receiving Report (Draft only)")
            .WithDescription("Updates header and line items of a PPE Receiving Report in Draft status.")
            .Produces<Application.PpeReceiving.Update.v1.UpdatePpeReceivingReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
