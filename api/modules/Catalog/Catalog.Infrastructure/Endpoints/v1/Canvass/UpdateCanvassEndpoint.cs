using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class UpdateCanvassEndpoint
{
    internal static RouteHandlerBuilder MapCanvassUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateCanvassCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("Route ID does not match request ID");

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateCanvassEndpoint))
            .WithSummary("Update canvass")
            .WithDescription("Updates an existing canvass quotation")
            .Produces<UpdateCanvassResponse>()
            .RequirePermission("Permissions.Canvasses.Update")
            .MapToApiVersion(1);
    }
}
