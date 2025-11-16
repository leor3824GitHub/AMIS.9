using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Employees.Search.v1;
public class SearchEmployeeSpecs : EntitiesByPaginationFilterSpec<Employee, EmployeeResponse>
{
    public SearchEmployeeSpecs(SearchEmployeesCommand command)
        : base(command)
    {
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword))
            .Where(b => b.UserId == command.UsrId!.Value, command.UsrId.HasValue);
        
        Query.Select(e => new EmployeeResponse(
            e.Id,
            e.Name,
            e.Designation,
            e.ResponsibilityCode,
            e.Department,
            e.ContactInfo.Email,
            e.ContactInfo.PhoneNumber,
            e.Status,
            e.HireDate,
            e.TerminationDate,
            e.SupervisorId,
            e.UserId));
    }
}
