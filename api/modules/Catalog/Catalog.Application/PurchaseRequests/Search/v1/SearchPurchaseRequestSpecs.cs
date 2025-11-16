using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using Ardalis.Specification;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;

public sealed class SearchPurchaseRequestSpecs : EntitiesByPaginationFilterSpec<PurchaseRequest, PurchaseRequestResponse>
{
    public SearchPurchaseRequestSpecs(SearchPurchaseRequestsCommand command)
        : base(command) =>
        Query
            .Include(r => r.Items)
            .OrderBy(r => r.RequestDate, !command.HasOrderBy())
            .Where(r => r.Status == command.Status!.Value, command.Status.HasValue)
            .Where(r => r.RequestedBy == command.RequestedBy!.Value, command.RequestedBy.HasValue)
            .Where(r => r.Purpose!.Contains(command.Keyword!), !string.IsNullOrWhiteSpace(command.Keyword));
}
