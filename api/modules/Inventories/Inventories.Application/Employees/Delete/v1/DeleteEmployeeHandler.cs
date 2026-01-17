using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.Employees.Delete.v1;
public sealed class DeleteEmployeeHandler(
    ILogger<DeleteEmployeeHandler> logger,
    [FromKeyedServices("inventories:employees")] IRepository<Employee> repository)
    : IRequestHandler<DeleteEmployeeCommand>
{
    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = employee ?? throw new EmployeeNotFoundException(request.Id);
        await repository.DeleteAsync(employee, cancellationToken);
        logger.LogInformation("Employee with id : {EmployeeId} deleted", employee.Id);
    }
}

