using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Employee;
public static class GetEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapGetEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetEmployeeRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetEmployeeEndpoint))
            .WithSummary("gets employee by id")
            .WithDescription("gets employee by id")
            .Produces<EmployeeResponse>()
            .RequirePermission("Permissions.Employees.View")
            .MapToApiVersion(1);
    }
}

