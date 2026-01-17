using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Employees.Update.v1;
public sealed class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> repository)
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse>
{
    public async Task<UpdateEmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = employee ?? throw new EmployeeNotFoundException(request.Id);
        var updatedEmployee = employee.Update(request.Name!, request.Designation, request.ResponsibilityCode, request.UserId);
        await repository.UpdateAsync(updatedEmployee, cancellationToken);
        logger.LogInformation("Employee with id : {EmployeeId} updated.", employee.Id);
        return new UpdateEmployeeResponse(employee.Id);
    }
}

