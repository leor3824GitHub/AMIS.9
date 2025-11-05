using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public class SearchInspectionRequestSpecs : EntitiesByPaginationFilterSpec<InspectionRequest, InspectionRequestResponse>
{
    public SearchInspectionRequestSpecs(SearchInspectionRequestsCommand command)
        : base(command)
    {
        Query
            .Include(i => i.Inspector)
            .Include(i => i.Purchase);

        if (command.PurchaseId.HasValue)
        {
            Query.Where(i => i.PurchaseId == command.PurchaseId.Value);
        }

        // Guard against nullable date comparisons producing bool? by lifting
        if (command.FromDate.HasValue)
        {
            var from = command.FromDate.Value;
            Query.Where(i => i.DateCreated >= from);
        }
        if (command.ToDate.HasValue)
        {
            var to = command.ToDate.Value;
            Query.Where(i => i.DateCreated <= to);
        }
    }
}
