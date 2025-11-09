using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public class SearchInspectionRequestSpecs : EntitiesByPaginationFilterSpec<InspectionRequest, InspectionRequestResponse>
{
    public SearchInspectionRequestSpecs(SearchInspectionRequestsCommand command)
        : base(command)
    {
        Query
           .Include(i => i.Inspector)
             .Include(i => i.Purchase)
           .ThenInclude(p => p!.Supplier);

        if (command.PurchaseId.HasValue)
       {
             Query.Where(i => i.PurchaseId == command.PurchaseId.Value);
            }

        // Guard against nullable date comparisons producing bool? by lifting
            if (command.FromDate.HasValue)
            {
           var from = command.FromDate.Value;
            Query.Where(i => i.DateCreated >= from);
            }
         if (command.ToDate.HasValue)
         {
                var to = command.ToDate.Value;
          Query.Where(i => i.DateCreated <= to);
            }

            // Explicit projection to InspectionRequestResponse to avoid deserialization issues
            Query.Select(i => new InspectionRequestResponse(
                i.Id,
                i.PurchaseId,
                i.InspectorId,
                i.Status,
                i.DateCreated,
                i.Purchase == null ? null! : new PurchaseResponse(
                  i.Purchase.Id,
                  i.Purchase.SupplierId,
                  i.Purchase.PurchaseDate,
     i.Purchase.TotalAmount,
                i.Purchase.Status,
 i.Purchase.Supplier == null ? null : new SupplierResponse(
i.Purchase.Supplier.Id,
        i.Purchase.Supplier.Name,
             i.Purchase.Supplier.Address,
       i.Purchase.Supplier.Tin,
       i.Purchase.Supplier.TaxClassification,
    i.Purchase.Supplier.ContactNo,
       i.Purchase.Supplier.Emailadd
       ),
     null, // Items
              i.Purchase.ReferenceNumber,
    i.Purchase.Created.DateTime, // CreatedOn - convert DateTimeOffset to DateTime
         i.Purchase.Notes,
                i.Purchase.Currency
     ),
     i.Inspector == null ? null! : new EmployeeResponse(
     i.Inspector.Id,
         i.Inspector.Name,
        i.Inspector.Designation,
     i.Inspector.ResponsibilityCode,
       i.Inspector.UserId
            )
        ));
    }
}
