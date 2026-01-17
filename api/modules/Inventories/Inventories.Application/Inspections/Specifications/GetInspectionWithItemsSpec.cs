//csharp api/modules/Catalog/Catalog.Application/Inspections/Specifications/GetInspectionWithItemsSpec.cs
using Ardalis.Specification;

namespace AMIS.WebApi.Inventories.Application.Inspections.Specifications;

using AMIS.WebApi.Inventories.Domain;

public sealed class GetInspectionWithItemsSpec : Specification<Inspection>
{
    public GetInspectionWithItemsSpec(Guid inspectionId)
    {
        Query.Where(i => i.Id == inspectionId)
             .Include(i => i.Items)
             .ThenInclude(it => it.PurchaseItem);
    }
}

