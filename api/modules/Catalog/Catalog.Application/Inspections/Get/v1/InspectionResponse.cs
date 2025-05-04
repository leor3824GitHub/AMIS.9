namespace AMIS.WebApi.Catalog.Application.Inspections.Get.v1;

public sealed record InspectionResponse(
    Guid Id,
    DateTime InspectionDate,
    string InspectedByName,
    string Remarks,
    Guid PurchaseOrderId
);

