using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;
public static class GetInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapGetInspectionRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetInspectionRequestRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetInspectionRequestEndpoint))
            .WithSummary("gets inspectionRequest by id")
            .WithDescription("gets inspectionRequest by id")
            .Produces<InspectionRequestResponse>()
            .RequirePermission("Permissions.InspectionRequests.View")
            .MapToApiVersion(1);
    }
}
