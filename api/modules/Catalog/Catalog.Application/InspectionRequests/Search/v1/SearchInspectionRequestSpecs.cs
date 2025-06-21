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
            .Include(i => i.Purchase)
            .Where(i => i.PurchaseId == command.PurchaseId!.Value, command.PurchaseId.HasValue)
            .Where(i => i.DateCreated >= command.FromDate, command.FromDate.HasValue)
            .Where(i => i.DateCreated <= command.ToDate, command.ToDate.HasValue);
    }
}
