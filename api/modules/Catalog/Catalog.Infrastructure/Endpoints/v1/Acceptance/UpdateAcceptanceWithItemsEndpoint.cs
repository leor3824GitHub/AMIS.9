using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Acceptances.UpdateWithItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class UpdateAcceptanceWithItemsEndpoint
{
    internal static RouteHandlerBuilder MapUpdateAcceptanceWithItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/with-items", async (Guid id, UpdateAcceptanceWithItemsCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateAcceptanceWithItemsEndpoint))
            .WithSummary("Update an acceptance header and its items atomically")
            .WithDescription("Upserts acceptance items, applies deletions, and updates acceptance header fields in a single operation")
            .Produces<UpdateAcceptanceWithItemsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Acceptances.Update")
            .MapToApiVersion(1);
    }
}