namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Post.v1;

public sealed record PostSuppliesAndMaterialsIssuanceReportResponse(
    Guid Id,
    string SmirNumber,
    string Status,
    string Message);
