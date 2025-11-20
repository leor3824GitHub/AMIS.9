using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class DeleteCanvassEndpoint
{
    internal static RouteHandlerBuilder MapCanvassDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new DeleteCanvassCommand(id));
                return Results.Ok(response);
            })
            .WithName(nameof(DeleteCanvassEndpoint))
            .WithSummary("Delete canvass")
            .WithDescription("Deletes a canvass quotation")
            .Produces<DeleteCanvassResponse>()
            .RequirePermission("Permissions.Canvasses.Delete")
            .MapToApiVersion(1);
    }
}
