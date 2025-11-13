using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class UpdateIssuanceWithItemsEndpoint
{
    internal static RouteHandlerBuilder MapUpdateIssuanceWithItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/with-items", async (Guid id, UpdateIssuanceWithItemsCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateIssuanceWithItemsEndpoint))
            .WithSummary("Update an issuance header and its items atomically")
            .WithDescription("Upserts issuance items, applies deletions, and updates issuance header fields in a single operation")
            .Produces<UpdateIssuanceWithItemsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Issuances.Update")
            .MapToApiVersion(1);
    }
}