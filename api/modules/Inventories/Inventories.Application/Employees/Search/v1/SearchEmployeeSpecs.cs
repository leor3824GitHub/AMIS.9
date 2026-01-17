using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Employees.Search.v1;
public class SearchEmployeeSpecs : EntitiesByPaginationFilterSpec<Employee, EmployeeResponse>
{
    public SearchEmployeeSpecs(SearchEmployeesCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword))
            .Where(b => b.UserId == command.UsrId!.Value, command.UsrId.HasValue);
}

