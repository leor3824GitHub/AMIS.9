using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteInspectionCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteInspectionEndpoint))
            .WithSummary("deletes inspection by id")
            .WithDescription("deletes inspection by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Inspections.Delete")
            .MapToApiVersion(1);
    }
}
