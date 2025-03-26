using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateIssuanceItemEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateIssuanceItemCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateIssuanceItemEndpoint))
            .WithSummary("update a issuanceItem")
            .WithDescription("update a issuanceItem")
            .Produces<UpdateIssuanceItemResponse>()
            .RequirePermission("Permissions.IssuanceItems.Update")
            .MapToApiVersion(1);
    }
}
