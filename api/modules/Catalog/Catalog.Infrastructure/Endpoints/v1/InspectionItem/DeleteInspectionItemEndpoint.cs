using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionItems.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1
{
    public static class DeleteInspectionItemEndpoint
    {
        internal static RouteHandlerBuilder MapInspectionItemDeletionEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
                {
                    var command = new DeleteInspectionItemCommand(id);
                    var response = await mediator.Send(command);
                    return Results.Ok(response);
                })
                .WithName(nameof(DeleteInspectionItemEndpoint))
                .WithSummary("Deletes an inspection item")
                .WithDescription("Deletes an inspection item by Id")
                .Produces<DeleteInspectionItemResponse>()
                .RequirePermission("Permissions.InspectionItems.Delete")
                .MapToApiVersion(1);
        }
    }
}
