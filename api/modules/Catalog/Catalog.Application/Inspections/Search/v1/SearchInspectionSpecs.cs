using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public class SearchInspectionSpecs : EntitiesByPaginationFilterSpec<Inspection, InspectionResponse>
{
    public SearchInspectionSpecs(SearchInspectionsCommand command)
        : base(command)
    {
        Query
            .Include(i => i.Inspector)
            .Include(i => i.Items)
            .OrderBy(c => c.InspectionDate, !command.HasOrderBy())
            .Where(i => i.InspectorId == command.InspectorId!.Value, command.InspectorId.HasValue)
            .Where(i => i.InspectionDate >= command.FromDate, command.FromDate.HasValue)
            .Where(i => i.InspectionDate <= command.ToDate, command.ToDate.HasValue);
    }
}
