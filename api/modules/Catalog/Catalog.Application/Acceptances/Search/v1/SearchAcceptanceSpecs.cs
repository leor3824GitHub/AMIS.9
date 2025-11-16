using Ardalis.Specification;
using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using System.Linq;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Search.v1;

public class SearchAcceptanceSpecs : EntitiesByPaginationFilterSpec<Acceptance, AcceptanceResponse>
{
    public SearchAcceptanceSpecs(SearchAcceptancesCommand command)
        : base(command)
    {
        Query
            .Include(a => a.SupplyOfficer)
            .Include(a => a.Purchase)
            .Include(a => a.Items)
                .ThenInclude(item => item.PurchaseItem)
            .OrderBy(a => a.AcceptanceDate, !command.HasOrderBy())
            .Where(a => a.InspectionId == command.InspectionId!.Value, command.InspectionId.HasValue)
            .Where(a => a.PurchaseId == command.PurchaseId!.Value, command.PurchaseId.HasValue)
            .Where(a => a.AcceptanceDate >= command.FromDate, command.FromDate.HasValue)
            .Where(a => a.AcceptanceDate <= command.ToDate, command.ToDate.HasValue);
        
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
                    a.SupplyOfficer.Department,
                    a.SupplyOfficer.ContactInfo.Email,
                    a.SupplyOfficer.ContactInfo.PhoneNumber,
                    a.SupplyOfficer.Status,
                    a.SupplyOfficer.HireDate,
                    a.SupplyOfficer.TerminationDate,
                    a.SupplyOfficer.SupervisorId,
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
