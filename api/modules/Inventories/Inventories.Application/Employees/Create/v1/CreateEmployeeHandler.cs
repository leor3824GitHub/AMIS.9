using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Employees.Create.v1;
public sealed class CreateEmployeeHandler(
    ILogger<CreateEmployeeHandler> logger,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> repository)
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    public async Task<CreateEmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var employee = Employee.Create(request.Name!, request.Designation, request.ResponsibilityCode!, request.UserId);
        await repository.AddAsync(employee, cancellationToken);
        logger.LogInformation("employee created {EmployeeId}", employee.Id);
        return new CreateEmployeeResponse(employee.Id);
    }
}

