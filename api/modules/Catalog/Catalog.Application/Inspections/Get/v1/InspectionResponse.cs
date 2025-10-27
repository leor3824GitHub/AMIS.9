using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public sealed record InspectionResponse(
    Guid Id,
    DateTime InspectedOn,
    Guid EmployeeId,
    Guid? PurchaseId,
    string? Remarks,
    EmployeeResponse Employee,
    PurchaseResponse? Purchase,
    bool Approved,
    InspectionStatus Status
);

