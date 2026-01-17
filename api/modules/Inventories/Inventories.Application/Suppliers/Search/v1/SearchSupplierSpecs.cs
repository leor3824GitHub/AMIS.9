using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Application.Suppliers.Get.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Suppliers.Search.v1;
public class SearchSupplierSpecs : EntitiesByPaginationFilterSpec<Supplier, SupplierResponse>
{
    public SearchSupplierSpecs(SearchSuppliersCommand command)
        : base(command) =>
        Query
            .OrderBy(c => c.Name, !command.HasOrderBy())
            .Where(b => b.Name.Contains(command.Keyword), !string.IsNullOrEmpty(command.Keyword));
}

