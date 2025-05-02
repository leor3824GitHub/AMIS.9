using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInspectionCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInspectionEndpoint))
            .WithSummary("update a inspection")
            .WithDescription("update a inspection")
            .Produces<UpdateInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Update")
            .MapToApiVersion(1);
    }
}
