using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateAcceptanceEndpoint
{
    internal static RouteHandlerBuilder MapAcceptanceUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateAcceptanceCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAcceptanceEndpoint))
            .WithSummary("update a acceptance")
            .WithDescription("update a acceptance")
            .Produces<UpdateAcceptanceResponse>()
            .RequirePermission("Permissions.Acceptances.Update")
            .MapToApiVersion(1);
    }
}
