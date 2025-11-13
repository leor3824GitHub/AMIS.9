using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.UpdateWithItems.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class UpdateInspectionWithItemsEndpoint
{
    internal static RouteHandlerBuilder MapUpdateInspectionWithItemsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/with-items", async (Guid id, UpdateInspectionWithItemsCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInspectionWithItemsEndpoint))
            .WithSummary("Update an inspection header and its items atomically")
            .WithDescription("Upserts inspection items, applies deletions, and updates inspection header fields in a single operation")
            .Produces<UpdateInspectionWithItemsResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}