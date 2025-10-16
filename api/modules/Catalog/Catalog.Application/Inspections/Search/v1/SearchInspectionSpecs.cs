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
            .Include(i => i.Employee)
            .Include(i => i.Purchase)
            .ThenInclude(p => p!.Supplier)
            .OrderBy(c => c.InspectedOn, !command.HasOrderBy())
            .Where(i => i.EmployeeId == command.InspectorId!.Value, command.InspectorId.HasValue)
            .Where(i => i.InspectedOn >= command.FromDate, command.FromDate.HasValue)
            .Where(i => i.InspectedOn <= command.ToDate, command.ToDate.HasValue);
    }
}
