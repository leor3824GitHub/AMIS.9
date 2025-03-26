using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteIssuanceItemEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceItemDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteIssuanceItemCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteIssuanceItemEndpoint))
            .WithSummary("deletes issuanceItem by id")
            .WithDescription("deletes issuanceItem by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.IssuanceItems.Delete")
            .MapToApiVersion(1);
    }
}
