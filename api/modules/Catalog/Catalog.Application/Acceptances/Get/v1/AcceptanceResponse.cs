namespace AMIS.WebApi.Catalog.Application.Acceptances.Get.v1;

public sealed record AcceptanceResponse(
    Guid Id,
    Guid PurchaseOrderId,
    string AcceptedByName,
    DateTime AcceptanceDate,    
    string Remarks    
);

