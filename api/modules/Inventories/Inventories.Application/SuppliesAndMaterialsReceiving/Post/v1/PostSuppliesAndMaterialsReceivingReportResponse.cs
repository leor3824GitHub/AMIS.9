namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Post.v1;

public sealed record PostSuppliesAndMaterialsReceivingReportResponse(
    Guid Id,
    string SmrrNumber,
    string Status,
    string Message);
