using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Employees.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapEmployeeUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateEmployeeCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateEmployeeEndpoint))
            .WithSummary("update a employee")
            .WithDescription("update a employee")
            .Produces<UpdateEmployeeResponse>()
            .RequirePermission("Permissions.Employees.Update")
            .MapToApiVersion(1);
    }
}
