using AMIS.WebApi.Inventories.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.Employees.Search.v1;

public sealed class EmployeeByUserIdSpec : Specification<Employee>
{
    public EmployeeByUserIdSpec(Guid userId)
    {
        Query.Where(e => e.UserId == userId);
    }
}

