using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Acceptances.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Acceptance
{
    public static class DeleteAcceptanceEndpoint
    {
        internal static RouteHandlerBuilder MapAcceptanceDeletionEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var command = new DeleteAcceptanceCommand(id);
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(DeleteAcceptanceEndpoint))
                .WithSummary("Deletes an acceptance")
                .WithDescription("Deletes an acceptance by Id")
                .Produces<DeleteAcceptanceResponse>()
                .RequirePermission("Permissions.Acceptances.Delete")
                .MapToApiVersion(1);
        }
    }
}

