using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class GetCanvassEndpoint
{
    internal static RouteHandlerBuilder MapGetCanvassEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCanvassRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetCanvassEndpoint))
            .WithSummary("Get canvass by ID")
            .WithDescription("Retrieves a specific canvass by its unique identifier")
            .Produces<CanvassResponse>()
            .RequirePermission("Permissions.Canvasses.View")
            .MapToApiVersion(1);
    }
}
