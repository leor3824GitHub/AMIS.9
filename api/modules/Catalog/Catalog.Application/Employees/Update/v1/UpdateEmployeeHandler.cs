using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Employees.Update.v1;

public sealed class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    [FromKeyedServices("catalog:employees")] IRepository<Employee> repository,
    ICurrentUser currentUser)
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse>
{
    public async Task<UpdateEmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = employee ?? throw new EmployeeNotFoundException(request.Id);

        // Get UserId from current authenticated user instead of request parameter
        var userId = currentUser.GetUserId();

        var updatedEmployee = employee.Update(request.Name!, request.Designation, request.ResponsibilityCode, userId);
        await repository.UpdateAsync(updatedEmployee, cancellationToken);
        logger.LogInformation("Employee with id : {EmployeeId} updated for user {UserId}", employee.Id, userId);
        return new UpdateEmployeeResponse(employee.Id);
    }
}
