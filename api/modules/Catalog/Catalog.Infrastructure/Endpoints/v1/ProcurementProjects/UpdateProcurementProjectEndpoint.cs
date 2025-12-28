using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.ProcurementProjects.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.ProcurementProjects;

public static class UpdateProcurementProjectEndpoint
{
    internal static RouteHandlerBuilder MapUpdateProcurementProjectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateProcurementProjectCommand request, ISender mediator) =>
            {
                request.Id = id;
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateProcurementProjectEndpoint))
            .WithSummary("update procurement project")
            .WithDescription("updates a procurement project including schedule and budget")
            .Produces<UpdateProcurementProjectResponse>()
            .RequirePermission("Permissions.ProcurementProjects.Update")
            .MapToApiVersion(1);
    }
}
