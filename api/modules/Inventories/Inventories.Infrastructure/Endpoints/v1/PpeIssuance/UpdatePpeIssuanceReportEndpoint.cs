using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public sealed class UpdatePpeIssuanceReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-issuance")
            .WithTags("PPE Issuance");

        group.MapPut("/{id:guid}", async (Guid id, Application.PpeIssuance.Update.v1.UpdatePpeIssuanceReportCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePpeIssuanceReportEndpoint))
            .WithSummary("Update a PPE Issuance Report (Draft only)")
            .WithDescription("Updates header and line items of a PPE Issuance Report in Draft status.")
            .Produces<Application.PpeIssuance.Update.v1.UpdatePpeIssuanceReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
