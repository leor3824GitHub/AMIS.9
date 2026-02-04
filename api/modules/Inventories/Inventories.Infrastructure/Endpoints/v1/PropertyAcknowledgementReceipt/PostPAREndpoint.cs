using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class PostPAREndpoint
{
    public static RouteHandlerBuilder MapPostPAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/post", async (Guid id, ISender mediator) =>
            {
                var command = new PostPARCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PostPAREndpoint))
            .WithSummary("Post a Property Acknowledgement Receipt")
            .WithDescription("Posts a draft PAR, assigning assets to the custodian.")
            .Produces<PostPARResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Post}")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
