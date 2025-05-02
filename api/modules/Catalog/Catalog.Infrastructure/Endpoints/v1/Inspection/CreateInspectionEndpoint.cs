using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateInspectionEndpoint
{
    internal static RouteHandlerBuilder MapInspectionCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateInspectionCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateInspectionEndpoint))
            .WithSummary("creates a inspection")
            .WithDescription("creates a inspection")
            .Produces<CreateInspectionResponse>()
            .RequirePermission("Permissions.Inspections.Create")
            .MapToApiVersion(1);
    }
}
