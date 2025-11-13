using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using System.Linq;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public class SearchInspectionSpecs : EntitiesByPaginationFilterSpec<Inspection, InspectionResponse>
{
    public SearchInspectionSpecs(SearchInspectionsCommand command)
        : base(command)
    {
        Query
            .Include(i => i.Employee)
            .Include(i => i.Purchase)
            .Include(i => i.Items)
                .ThenInclude(item => item.PurchaseItem)
            .OrderBy(c => c.InspectedOn, !command.HasOrderBy())
            .Where(i => i.EmployeeId == command.InspectorId!.Value, command.InspectorId.HasValue)
            .Where(i => i.InspectedOn >= command.FromDate, command.FromDate.HasValue)
            .Where(i => i.InspectedOn <= command.ToDate, command.ToDate.HasValue);
        
        Query.PostProcessingAction(inspections => inspections
            .Select(i => new InspectionResponse(
                i.Id,
                i.InspectedOn,
                i.EmployeeId,
                i.PurchaseId,
                i.Remarks,
                new EmployeeResponse(i.Employee.Id, i.Employee.Name, i.Employee.Designation, i.Employee.ResponsibilityCode, i.Employee.UserId),
                i.Purchase == null ? null : new PurchaseResponse(
                    i.Purchase.Id,
                    i.Purchase.SupplierId,
                    i.Purchase.PurchaseDate,
                    i.Purchase.TotalAmount,
                    i.Purchase.Status,
                    null,
                    null
                ),
                i.Approved,
                i.Status,
                i.Items.Select(item => new InspectionItemResponse(
                    item.Id,
                    item.InspectionId,
                    item.PurchaseItemId,
                    item.QtyInspected,
                    item.QtyPassed,
                    item.QtyFailed,
                    item.Remarks,
                    item.InspectionItemStatus
                )).ToList()
            ))
        );
    }
}
