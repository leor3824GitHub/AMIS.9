using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class PostAcceptanceEndpoint
{
    internal static RouteHandlerBuilder MapAcceptancePostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/post", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new PostAcceptanceCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(PostAcceptanceEndpoint))
            .WithSummary("post an acceptance")
            .WithDescription("mark an acceptance as posted")
            .Produces<PostAcceptanceResponse>()
            .RequirePermission("Permissions.Acceptances.Post")
            .MapToApiVersion(1);
    }
}
