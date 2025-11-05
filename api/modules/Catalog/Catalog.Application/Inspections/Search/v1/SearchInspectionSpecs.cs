using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Specifications;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public class SearchInspectionSpecs : EntitiesByPaginationFilterSpec<Inspection, InspectionResponse>
{
    public SearchInspectionSpecs(SearchInspectionsCommand command)
        : base(command)
    {
        Query
 .Include(i => i.Employee)
     .Include(i => i.InspectionRequest)
     .ThenInclude(ir => ir!.Purchase)
       .OrderBy(c => c.InspectedOn, !command.HasOrderBy());

        if (command.InspectorId.HasValue)
{
  Query.Where(i => i.EmployeeId == command.InspectorId.Value);
        }

        // Note: avoid lifted comparisons with nullable DateTime that create bool? expressions
        // and can throw when building the expression tree if the nullable has no value.
  if (command.FromDate.HasValue)
    {
         var from = command.FromDate.Value;
      Query.Where(i => i.InspectedOn.HasValue && i.InspectedOn.Value >= from);
        }

        if (command.ToDate.HasValue)
        {
      var to = command.ToDate.Value;
       Query.Where(i => i.InspectedOn.HasValue && i.InspectedOn.Value <= to);
  }

 // Explicit projection to InspectionResponse to avoid Mapster projecting legacy NULLs into non-nullable members
        Query.Select(i => new InspectionResponse(
       i.Id,
    i.InspectedOn,
    i.EmployeeId,
            i.InspectionRequest != null && i.InspectionRequest.PurchaseId.HasValue ? i.InspectionRequest.PurchaseId : null,
   i.Remarks,
    new EmployeeResponse(
          i.Employee != null ? i.Employee.Id : (Guid?)null,
           i.Employee != null ? i.Employee.Name : string.Empty,
    i.Employee != null ? i.Employee.Designation : string.Empty,
    i.Employee != null ? i.Employee.ResponsibilityCode : string.Empty,
      i.Employee != null ? i.Employee.UserId : null
      ),
   i.InspectionRequest == null || i.InspectionRequest.Purchase == null ? null : new PurchaseResponse(
         i.InspectionRequest.Purchase.Id,
           i.InspectionRequest.Purchase.SupplierId,
     i.InspectionRequest.Purchase.PurchaseDate,
         i.InspectionRequest.Purchase.TotalAmount,
   i.InspectionRequest.Purchase.Status,
          null,
     null
        ),
      i.Approved,
        i.Status
        ));
    }
}
