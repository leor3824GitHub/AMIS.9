using AMIS.WebApi.Catalog.Application.Employees.SelfRegister.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Employee;

public static class SelfRegisterEmployeeEndpoint
{
    internal static RouteHandlerBuilder MapSelfRegisterEmployeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/self-register", async (SelfRegisterEmployeeCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Created($"/api/v1/employees/check-registration", response);
            })
            .WithName(nameof(SelfRegisterEmployeeEndpoint))
            .WithSummary("Self-register employee information")
            .WithDescription("Allows authenticated users to register their employee information")
            .Produces<SelfRegisterEmployeeResponse>(StatusCodes.Status201Created)
            .RequireAuthorization()
            .MapToApiVersion(1);
    }
}
