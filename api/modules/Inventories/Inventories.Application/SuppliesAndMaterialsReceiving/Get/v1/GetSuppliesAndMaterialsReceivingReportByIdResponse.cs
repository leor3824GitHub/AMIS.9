namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Get.v1;

public sealed record GetSuppliesAndMaterialsReceivingReportByIdResponse(
    Guid Id,
    string SmrrNumber,
    string TransactionType,
    decimal TotalAmount,
    string? Notes);

