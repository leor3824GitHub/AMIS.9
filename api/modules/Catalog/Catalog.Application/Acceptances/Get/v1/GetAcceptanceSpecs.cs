using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using System.Linq;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;

public class GetAcceptanceSpecs : Specification<Acceptance, AcceptanceResponse>
{
    public GetAcceptanceSpecs(Guid id)
    {
        Query
            .Where(a => a.Id == id)
            .Include(a => a.Purchase)
            .Include(a => a.Items)
                .ThenInclude(item => item.PurchaseItem)
            .Include(a => a.SupplyOfficer);
        
        Query.Select(a => new AcceptanceResponse(
                a.Id,
                a.PurchaseId,
                a.SupplyOfficerId,
                a.AcceptanceDate,
                a.Remarks ?? string.Empty,
                a.IsPosted,
                a.PostedOn,
                a.Status,
                new EmployeeResponse(
                    a.SupplyOfficer.Id,
                    a.SupplyOfficer.Name,
                    a.SupplyOfficer.Designation,
                    a.SupplyOfficer.ResponsibilityCode,
                    a.SupplyOfficer.UserId),
                a.Items.Select(item => new AcceptanceItemResponse(
                    item.Id,
                    item.AcceptanceId,
                    item.PurchaseItemId,
                    item.QtyAccepted,
                    item.Remarks
                )).ToList()
            ));
    }
}
