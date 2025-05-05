using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionItems.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateInspectionItemEndpoint
{
    internal static RouteHandlerBuilder MapInspectionItemUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInspectionItemCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInspectionItemEndpoint))
            .WithSummary("update a inspection")
            .WithDescription("update a inspection")
            .Produces<UpdateInspectionItemResponse>()
            .RequirePermission("Permissions.InspectionItems.Update")
            .MapToApiVersion(1);
    }
}
