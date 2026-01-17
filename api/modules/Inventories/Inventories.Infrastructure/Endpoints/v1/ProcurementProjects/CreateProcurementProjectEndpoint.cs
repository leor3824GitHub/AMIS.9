using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.ProcurementProjects.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.ProcurementProjects;

public static class CreateProcurementProjectEndpoint
{
    internal static RouteHandlerBuilder MapProcurementProjectCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateProcurementProjectCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateProcurementProjectEndpoint))
            .WithSummary("create procurement project")
            .WithDescription("creates a procurement project with schedule and budget")
            .Produces<CreateProcurementProjectResponse>()
            .RequirePermission("Permissions.ProcurementProjects.Create")
            .MapToApiVersion(1);
    }
}

