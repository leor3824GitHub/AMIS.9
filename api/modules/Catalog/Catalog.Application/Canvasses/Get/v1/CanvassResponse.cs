namespace AMIS.WebApi.Catalog.Application.Canvasses.Get.v1;

public sealed record CanvassResponse(
    Guid Id,
    Guid PurchaseRequestId,
    Guid SupplierId,
    string ItemDescription,
    int Quantity,
    string Unit,
    decimal QuotedPrice,
    string? Remarks,
    DateTime ResponseDate,
    bool IsSelected);
