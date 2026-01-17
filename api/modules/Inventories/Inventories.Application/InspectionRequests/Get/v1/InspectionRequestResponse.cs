using AMIS.WebApi.Inventories.Application.Employees.Get.v1;
using AMIS.WebApi.Inventories.Application.Purchases.Get.v1;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.Get.v1;

public sealed record InspectionRequestResponse(
    Guid? Id,
    Guid? PurchaseId,
    Guid? InspectorId,
    InspectionRequestStatus Status,
    DateTime DateCreated,
    PurchaseResponse Purchase,
    EmployeeResponse Inspector
);

