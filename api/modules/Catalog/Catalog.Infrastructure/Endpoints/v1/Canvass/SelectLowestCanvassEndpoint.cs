using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Select.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class SelectLowestCanvassEndpoint
{
    internal static RouteHandlerBuilder MapCanvassSelectLowestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/select-lowest", async (SelectLowestCanvassCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(SelectLowestCanvassEndpoint))
            .WithSummary("Select lowest canvass")
            .WithDescription("Marks the lowest quoted canvass (with earliest response tie-breaker) as selected and unselects others")
            .Produces<SelectLowestCanvassResponse>()
            .RequirePermission("Permissions.Canvasses.Update")
            .MapToApiVersion(1);
    }
}
