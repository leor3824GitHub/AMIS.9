using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.WebApi.Catalog.Application.Employees.CheckRegistration.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.Employee;

public static class CheckEmployeeRegistrationEndpoint
{
    internal static RouteHandlerBuilder MapCheckEmployeeRegistrationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/check-registration", async (ISender mediator, ICurrentUser currentUser) =>
            {
                var userId = currentUser.GetUserId();
                var response = await mediator.Send(new CheckEmployeeRegistrationQuery(userId));
                return Results.Ok(response);
            })
            .WithName(nameof(CheckEmployeeRegistrationEndpoint))
            .WithSummary("Check if current user has registered employee information")
            .WithDescription("Returns registration status for the authenticated user")
            .Produces<EmployeeRegistrationStatusResponse>()
            .RequireAuthorization()
            .MapToApiVersion(1);
    }
}
