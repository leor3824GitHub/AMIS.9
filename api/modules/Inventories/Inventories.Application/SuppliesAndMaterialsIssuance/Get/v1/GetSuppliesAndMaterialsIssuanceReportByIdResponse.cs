namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Get.v1;

public sealed record GetSuppliesAndMaterialsIssuanceReportByIdResponse(
    Guid Id,
    string SmirNumber,
    DateTime TransactionDate,
    string IssuanceReason,
    decimal TotalAmount,
    string? Notes);

