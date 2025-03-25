using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Employees.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class DeleteEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapEmployeeDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteEmployeeCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteEmployeeEndpoint))
            .WithSummary("deletes employee by id")
            .WithDescription("deletes employee by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Employees.Delete")
            .MapToApiVersion(1);
    }
}
