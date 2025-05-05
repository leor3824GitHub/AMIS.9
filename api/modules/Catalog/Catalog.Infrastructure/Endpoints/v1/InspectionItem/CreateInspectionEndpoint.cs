using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.InspectionItems.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateInspectionItemEndpoint
{
    internal static RouteHandlerBuilder MapInspectionItemCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInspectionItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInspectionItemEndpoint))
            .WithSummary("creates a inspection")
            .WithDescription("creates a inspection")
            .Produces<CreateInspectionItemResponse>()
            .RequirePermission("Permissions.InspectionItems.Create")
            .MapToApiVersion(1);
    }
}
