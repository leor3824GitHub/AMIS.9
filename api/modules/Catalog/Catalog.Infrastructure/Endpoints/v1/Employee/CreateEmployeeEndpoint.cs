using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Employees.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapEmployeeCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateEmployeeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateEmployeeEndpoint))
            .WithSummary("creates a employee")
            .WithDescription("creates a employee")
            .Produces<CreateEmployeeResponse>()
            .RequirePermission("Permissions.Employees.Create")
            .MapToApiVersion(1);
    }
}
