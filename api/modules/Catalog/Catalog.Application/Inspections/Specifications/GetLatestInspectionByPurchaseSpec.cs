using System;
using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inspections.Specifications;

/// <summary>
/// Specification to get the latest inspection for a purchase.
/// Note: Navigates through InspectionRequest to access PurchaseId following DDD aggregate boundaries.
/// </summary>
public sealed class GetLatestInspectionByPurchaseSpec : Specification<Inspection>
{
    public GetLatestInspectionByPurchaseSpec(Guid purchaseId)
    {
        Query
            .Include(i => i.InspectionRequest)
            .Where(i => i.InspectionRequest != null && i.InspectionRequest.PurchaseId == purchaseId)
            .OrderByDescending(i => i.Created)
            .Take(1);
    }
}
