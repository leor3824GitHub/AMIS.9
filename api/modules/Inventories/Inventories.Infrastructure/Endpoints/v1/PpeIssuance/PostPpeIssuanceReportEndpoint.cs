using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class PostPpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapPostPpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/post", async (Guid id, ISender mediator) =>
            {
                var command = new Application.PpeIssuance.Post.v1.PostPpeIssuanceReportCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PostPpeIssuanceReportEndpoint))
            .WithSummary("Post a PPE Issuance Report")
            .WithDescription("Posts a draft PPEIR, making it immutable and updating inventory registry/logs.")
            .Produces<Application.PpeIssuance.Post.v1.PostPpeIssuanceReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.PpeIssuance.Post")
            .MapToApiVersion(1);
    }
}
