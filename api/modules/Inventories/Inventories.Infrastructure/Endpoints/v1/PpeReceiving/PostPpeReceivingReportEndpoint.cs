using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public sealed class PostPpeReceivingReportEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v{version:apiVersion}/ppe-receiving")
            .WithTags("PPE Receiving");

        group.MapPost("/{id:guid}/post", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeReceiving.Post.v1.PostPpeReceivingReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PostPpeReceivingReportEndpoint))
            .WithSummary("Post a PPE Receiving Report")
            .WithDescription("Posts a draft PPERR, making it immutable and updating inventory registry/logs.")
            .Produces<Application.PpeReceiving.Post.v1.PostPpeReceivingReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
