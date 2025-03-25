using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Employees.Search.v1;

public class SearchEmployeesCommand : PaginationFilter, IRequest<PagedList<EmployeeResponse>>
{
    public string? Name { get; set; }
    public string? Designation { get; set; }
    public string? ResponsibilityCode { get; set; }
    public Guid? UsrId { get; set; }

}
