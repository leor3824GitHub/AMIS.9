using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.InspectionRequest.v1;
public static class CreateInspectionRequestEndpoint
{
    internal static RouteHandlerBuilder MapInspectionRequestCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInspectionRequestCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInspectionRequestEndpoint))
            .WithSummary("creates a inspectionRequest")
            .WithDescription("creates a inspectionRequest")
            .Produces<CreateInspectionRequestResponse>()
            .RequirePermission("Permissions.InspectionRequests.Create")
            .MapToApiVersion(1);
    }
}
