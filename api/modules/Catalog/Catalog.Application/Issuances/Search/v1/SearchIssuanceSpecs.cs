using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Issuances.Search.v1;

public class SearchIssuanceSpecs : EntitiesByPaginationFilterSpec<Issuance, IssuanceResponse>
{
    public SearchIssuanceSpecs(SearchIssuancesCommand command)
        : base(command) =>
        Query
            .AsNoTracking()
            .Include(p => p.Employee)
            .OrderBy(c => c.Employee.Name, !command.HasOrderBy())
            .Where(p => p.EmployeeId == command.EmployeeId!.Value, command.EmployeeId.HasValue);
}
