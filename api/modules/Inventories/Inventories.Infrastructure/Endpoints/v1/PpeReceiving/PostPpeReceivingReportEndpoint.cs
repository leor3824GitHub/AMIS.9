using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class PostPpeReceivingReportEndpoint
{
    public static RouteHandlerBuilder MapPostPpeReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/post", async (Guid id, ISender mediator) =>
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
                .RequirePermission("Permissions.PpeReceiving.Post")
            .MapToApiVersion(1);
    }
}
