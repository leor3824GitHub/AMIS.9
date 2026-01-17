using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using AMIS.WebApi.Inventories.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Get.v1;

public sealed record AcceptanceResponse(
    Guid Id,
    Guid PurchaseId,
    Guid SupplyOfficerId,
    DateTime AcceptanceDate,    
    string Remarks,
    bool IsPosted,
    DateTime? PostedOn,
    AcceptanceStatus Status,
    EmployeeResponse? SupplyOfficer,
    ICollection<AcceptanceItemResponse>? Items
);


