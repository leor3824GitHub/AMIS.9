using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetInspectionItemEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInspectionItemRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInspectionItemEndpoint))
            .WithSummary("gets inspection item by id")
            .WithDescription("gets inspection item by id")
            .Produces<InspectionItemResponse>()
            .RequirePermission("Permissions.InspectionItems.View")
            .MapToApiVersion(1);
    }
}
