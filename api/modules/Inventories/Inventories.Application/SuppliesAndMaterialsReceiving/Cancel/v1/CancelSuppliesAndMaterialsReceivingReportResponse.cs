namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Cancel.v1;

public sealed record CancelSuppliesAndMaterialsReceivingReportResponse(
    Guid Id,
    string Status,
    string Message);
