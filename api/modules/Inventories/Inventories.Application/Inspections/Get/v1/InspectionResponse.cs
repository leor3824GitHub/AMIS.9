using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Inspections.Get.v1;

public sealed record InspectionResponse(
    Guid Id,
    DateTime InspectedOn,
    Guid EmployeeId,
    Guid? PurchaseId,
    string? Remarks,
    EmployeeResponse Employee,
    PurchaseResponse? Purchase,
    bool Approved,
    InspectionStatus Status,
    ICollection<InspectionItemResponse>? Items
);


