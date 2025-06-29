using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.Inspection.v1;
public static class GetInspectionEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInspectionRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInspectionEndpoint))
            .WithSummary("gets inspection by id")
            .WithDescription("gets inspection by id")
            .Produces<InspectionResponse>()
            .RequirePermission("Permissions.Inspections.View")
            .MapToApiVersion(1);
    }
}
