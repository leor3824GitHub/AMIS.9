using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Employees.CheckRegistration.v1;

public sealed class CheckEmployeeRegistrationHandler(
    [FromKeyedServices("catalog:employees")] IReadRepository<Employee> repository)
    : IRequestHandler<CheckEmployeeRegistrationQuery, EmployeeRegistrationStatusResponse>
{
    public async Task<EmployeeRegistrationStatusResponse> Handle(
        CheckEmployeeRegistrationQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new EmployeeByUserIdSpec(request.UserId);
        var employee = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (employee is null)
        {
            return new EmployeeRegistrationStatusResponse(
                false,
                null,
                "Employee information not found. Please complete your registration.");
        }

        return new EmployeeRegistrationStatusResponse(
            true,
            employee.Id,
            "Employee information is registered.");
    }
}
