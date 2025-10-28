using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Search.v1;
public class SearchInspectionItemSpecs : EntitiesByPaginationFilterSpec<InspectionItem, InspectionItemResponse>
{
    public SearchInspectionItemSpecs(SearchInspectionItemsCommand command)
        : base(command) =>
        Query
            .Include(p => p.Inspection)
            // Filter by InspectionId when provided
            .Where(p => p.InspectionId == command.InspectionId!.Value, command.InspectionId.HasValue)
            // Filter by PurchaseItemId when provided
            .Where(p => p.PurchaseItemId == command.PurchaseItemId!.Value, command.PurchaseItemId.HasValue)
            // Always order newest first to support deterministic "latest-only" lookups with PageSize = 1
            .OrderByDescending(p => p.Created);
}
