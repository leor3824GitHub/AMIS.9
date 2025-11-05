using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Search.v1;
public class SearchIssuanceItemSpecs : EntitiesByPaginationFilterSpec<IssuanceItem, IssuanceItemResponse>
{
    public SearchIssuanceItemSpecs(SearchIssuanceItemsCommand command)
        : base(command)
    {
        Query
            .Include(p => p.Product)
            .OrderBy(c => c.Product.Name, !command.HasOrderBy());

        if (command.IssuanceId.HasValue)
        {
            Query.Where(p => p.IssuanceId == command.IssuanceId.Value);
        }
    }
}
