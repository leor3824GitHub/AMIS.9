using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Get.v1;
public sealed class GetEmployeeHandler(
    [FromKeyedServices("catalog:employees")] IReadRepository<Employee> repository,
    ICacheService cache)
    : IRequestHandler<GetEmployeeRequest, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"employee:{request.Id}",
            async () =>
            {
                var employee = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (employee == null) throw new EmployeeNotFoundException(request.Id);
                return new EmployeeResponse(
                    employee.Id,
                    employee.Name,
                    employee.Designation,
                    employee.ResponsibilityCode,
                    employee.Department,
                    employee.ContactInfo.Email,
                    employee.ContactInfo.PhoneNumber,
                    employee.Status,
                    employee.HireDate,
                    employee.TerminationDate,
                    employee.SupervisorId,
                    employee.UserId);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
