using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;

public sealed record AcceptanceResponse(
    Guid Id,
    Guid PurchaseId,
    Guid SupplyOfficerId,
    DateTime AcceptanceDate,    
    string Remarks,
    EmployeeResponse SupplyOfficer,
    ICollection<AcceptanceItemResponse> Items
);

