using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteIssuanceEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteIssuanceCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteIssuanceEndpoint))
            .WithSummary("deletes issuance by id")
            .WithDescription("deletes issuance by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Issuances.Delete")
            .MapToApiVersion(1);
    }
}
