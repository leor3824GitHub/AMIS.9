using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Canvasses.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Canvass;

public static class CreateCanvassEndpoint
{
    internal static RouteHandlerBuilder MapCanvassCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateCanvassCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.CreatedAtRoute(nameof(GetCanvassEndpoint), new { id = response.Id }, response);
            })
            .WithName(nameof(CreateCanvassEndpoint))
            .WithSummary("Create a new canvass")
            .WithDescription("Creates a new supplier price quotation for a purchase request")
            .Produces<CreateCanvassResponse>(StatusCodes.Status201Created)
            .RequirePermission("Permissions.Canvasses.Create")
            .MapToApiVersion(1);
    }
}
