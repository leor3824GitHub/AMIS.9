using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeCategoryCode;

public static class GetPpeCategoryCodeEndpoint
{
    internal static RouteHandlerBuilder MapGetPpeCategoryCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetPpeCategoryCodeRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetPpeCategoryCodeEndpoint))
            .WithSummary("Gets PPE category code by id")
            .WithDescription("Gets PPE category code by id")
            .Produces<PpeCategoryCodeResponse>()
            .RequirePermission("Permissions.PpeCategoryCodes.View")
            .MapToApiVersion(1);
    }
}
