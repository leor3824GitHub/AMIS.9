using System;
using Ardalis.Specification;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Inspections.Specifications;

public sealed class GetLatestInspectionByPurchaseSpec : Specification<Inspection>
{
    public GetLatestInspectionByPurchaseSpec(Guid purchaseId)
    {
       Query.Where(i => i.PurchaseId == purchaseId)
           .OrderByDescending(i => i.Created)
             .Take(1);
    }
}
