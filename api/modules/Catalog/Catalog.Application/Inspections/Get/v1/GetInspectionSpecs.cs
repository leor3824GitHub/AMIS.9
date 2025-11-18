using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using System.Linq;

namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public class GetInspectionSpecs : Specification<Inspection, InspectionResponse>
{
    public GetInspectionSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.Purchase)
            .Include(i => i.Employee)
            .Include(i => i.Items)
                .ThenInclude(item => item.PurchaseItem);
        
        Query.Select(i => new InspectionResponse(
                i.Id,
                i.InspectedOn,
                i.EmployeeId,
                i.PurchaseId,
                i.Remarks,
                new EmployeeResponse(
                    i.Employee.Id,
                    i.Employee.Name,
                    i.Employee.Designation,
                    i.Employee.ResponsibilityCode,
                    i.Employee.UserId),
                i.Purchase == null ? null : new PurchaseResponse(
                    i.Purchase.Id,
                    i.Purchase.SupplierId,
                    i.Purchase.PurchaseDate,
                    i.Purchase.TotalAmount,
                    i.Purchase.Status,
                    null,
                    null,
                    i.Purchase.DeliveryAddress
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
            ));
    }
}
