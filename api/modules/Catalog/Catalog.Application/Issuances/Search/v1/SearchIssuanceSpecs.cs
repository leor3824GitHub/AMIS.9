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
            .Include(p => p.Employee)
            .Include(p => p.Product)
            .OrderBy(c => c.Product.Name, !command.HasOrderBy())
            .Where(p => p.ProductId == command.ProductId!.Value, command.ProductId.HasValue)
            .Where(p => p.EmployeeId == command.EmployeeId!.Value, command.EmployeeId.HasValue);
}
