using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Employees.Create.v1;

public sealed class CreateEmployeeHandler(
    ILogger<CreateEmployeeHandler> logger,
    [FromKeyedServices("catalog:employees")] IRepository<Employee> repository,
    ICurrentUser currentUser)
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    public async Task<CreateEmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Get UserId from current authenticated user instead of request parameter
        var userId = currentUser.GetUserId();

        var employee = Employee.Create(request.Name!, request.Designation, request.ResponsibilityCode!, userId);
        await repository.AddAsync(employee, cancellationToken);
        logger.LogInformation("employee created {EmployeeId} for user {UserId}", employee.Id, userId);
        return new CreateEmployeeResponse(employee.Id);
    }
}
