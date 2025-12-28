using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementProjects.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementProjects;

public static class GetProcurementProjectEndpoint
{
    internal static RouteHandlerBuilder MapGetProcurementProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProcurementProjectRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetProcurementProjectEndpoint))
            .WithSummary("get procurement project by id")
            .WithDescription("retrieves a procurement project including schedule and budget")
            .Produces<GetProcurementProjectResponse>()
            .RequirePermission("Permissions.ProcurementProjects.View")
            .MapToApiVersion(1);
    }
}
