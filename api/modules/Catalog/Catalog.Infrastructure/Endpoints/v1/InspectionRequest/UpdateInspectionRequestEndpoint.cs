using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;
public static class UpdateInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateInspectionRequestCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateInspectionRequestEndpoint))
            .WithSummary("update a inspection request")
            .WithDescription("update a inspection request")
            .Produces<UpdateInspectionRequestResponse>()
            .RequirePermission("Permissions.InspectionRequests.Update")
            .MapToApiVersion(1);
    }
}
