using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Employees.SelfRegister.v1;

public sealed class SelfRegisterEmployeeHandler(
    ILogger<SelfRegisterEmployeeHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("catalog:employees")] IRepository<Employee> repository)
    : IRequestHandler<SelfRegisterEmployeeCommand, SelfRegisterEmployeeResponse>
{
    public async Task<SelfRegisterEmployeeResponse> Handle(
        SelfRegisterEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var userId = currentUser.GetUserId();

        // Check if user already has an employee record
        var spec = new EmployeeByUserIdSpec(userId);
        var existingEmployee = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (existingEmployee is not null)
        {
            throw new FshException("Employee information already registered for this user.");
        }

        // Create employee with current user's ID
        var employee = Employee.Create(
            request.Name,
            request.Designation,
            request.ResponsibilityCode,
            userId);

        await repository.AddAsync(employee, cancellationToken);

        logger.LogInformation(
            "Employee self-registered: {EmployeeId} for User: {UserId}",
            employee.Id,
            userId);

        return new SelfRegisterEmployeeResponse(
            employee.Id,
            "Employee information successfully registered.");
    }
}
