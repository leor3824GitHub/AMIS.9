using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Delete.v1;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1
{
    public static class DeleteAcceptanceItemEndpoint
    {
        internal static RouteHandlerBuilder MapAcceptanceItemDeletionEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var command = new DeleteAcceptanceItemCommand(id);
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(DeleteAcceptanceItemEndpoint))
                .WithSummary("Deletes an acceptance item")
                .WithDescription("Deletes an acceptance item by Id")
                .Produces<DeleteAcceptanceItemResponse>()
                .RequirePermission("Permissions.AcceptanceItems.Delete")
                .MapToApiVersion(1);
        }
    }
}
