using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Search.v1;
public class SearchAcceptanceItemSpecs : EntitiesByPaginationFilterSpec<AcceptanceItem, AcceptanceItemResponse>
{
    public SearchAcceptanceItemSpecs(SearchAcceptanceItemsCommand command)
        : base(command)
    {
        Query
            .Include(p => p.Acceptance);

        if (command.AcceptanceId.HasValue)
        {
            Query.Where(p => p.AcceptanceId == command.AcceptanceId.Value);
        }
    }
}
