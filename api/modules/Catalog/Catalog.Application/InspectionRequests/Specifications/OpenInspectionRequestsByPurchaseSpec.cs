using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Specifications;

public sealed class OpenInspectionRequestsByPurchaseSpec : Specification<InspectionRequest>
{
    public OpenInspectionRequestsByPurchaseSpec(Guid purchaseId)
    {
        Query
            .Where(ir => ir.PurchaseId == purchaseId)
            .Where(ir => ir.Status == InspectionRequestStatus.Pending
                      || ir.Status == InspectionRequestStatus.Assigned
                      || ir.Status == InspectionRequestStatus.InProgress);
    }
}
