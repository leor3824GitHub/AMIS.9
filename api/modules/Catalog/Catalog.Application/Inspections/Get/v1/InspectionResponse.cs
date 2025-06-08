using AMIS.WebApi.Catalog.Application.Employees.Get.v1;

namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public sealed record InspectionResponse(
    Guid Id,
    DateTime InspectionDate,
    Guid InspectorId,
    Guid PurchaseOrderId,
    string Remarks,
    EmployeeResponse Inspector
);

