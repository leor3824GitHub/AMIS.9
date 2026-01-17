using Ardalis.Specification;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Specifications;

public sealed class GetInspectionRequestByPurchaseSpec : Specification<InspectionRequest>
{
    public GetInspectionRequestByPurchaseSpec(Guid? purchaseId)
    {
        Query.Where(ir => ir.PurchaseId == purchaseId)
             .Include(ir => ir.Inspector)
             .Include(ir => ir.Purchase);
    }
}
