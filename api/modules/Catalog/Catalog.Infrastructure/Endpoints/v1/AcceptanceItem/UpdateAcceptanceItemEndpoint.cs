using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateAcceptanceItemEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateAcceptanceItemCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAcceptanceItemEndpoint))
            .WithSummary("update a acceptance item")
            .WithDescription("update a acceptance item")
            .Produces<UpdateAcceptanceItemResponse>()
            .RequirePermission("Permissions.AcceptanceItems.Update")
            .MapToApiVersion(1);
    }
}
