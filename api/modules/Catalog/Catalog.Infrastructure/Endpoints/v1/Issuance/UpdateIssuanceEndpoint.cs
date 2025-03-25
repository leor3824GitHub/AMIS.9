using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateIssuanceEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateIssuanceCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateIssuanceEndpoint))
            .WithSummary("update a product")
            .WithDescription("update a product")
            .Produces<UpdateIssuanceResponse>()
            .RequirePermission("Permissions.Issuances.Update")
            .MapToApiVersion(1);
    }
}
