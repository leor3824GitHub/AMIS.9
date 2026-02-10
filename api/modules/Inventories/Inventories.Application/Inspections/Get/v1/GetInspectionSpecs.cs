using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
using System.Linq;

namespace AMIS.WebApi.Inventories.Application.Inspections.Get.v1;

public class GetInspectionSpecs : Specification<Inspection, InspectionResponse>
{
    public GetInspectionSpecs(Guid id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.Purchase)
            .Include(i => i.PhysicalAsset)
            .Include(i => i.Employee)
            .Include(i => i.Items)
                .ThenInclude(item => item.PurchaseItem);

        Query.Select(i => new InspectionResponse(
                i.Id,
                i.Type,
                i.InspectedOn,
                i.EmployeeId,
                i.PurchaseId,
                i.PhysicalAssetId,
                i.Remarks,
                i.IARDocumentPath,
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


