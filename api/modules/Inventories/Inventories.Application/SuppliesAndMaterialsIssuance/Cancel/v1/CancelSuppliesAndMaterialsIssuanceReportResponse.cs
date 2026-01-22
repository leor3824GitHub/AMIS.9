namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Cancel.v1;

public sealed record CancelSuppliesAndMaterialsIssuanceReportResponse(
    Guid Id,
    string Status,
    string Message);
