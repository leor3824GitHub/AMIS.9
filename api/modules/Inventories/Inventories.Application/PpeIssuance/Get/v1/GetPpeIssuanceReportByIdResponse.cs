namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed record GetPpeIssuanceReportByIdResponse(
    Guid Id,
    string ReportNumber,
    string RecipientName,
    string RecipientAddress,
    string IssuanceType,
    DateTime IssuanceDate,
    decimal TotalAcquisitionCost,
    int LineItemCount,
    DateTime CreatedOnUtc);

