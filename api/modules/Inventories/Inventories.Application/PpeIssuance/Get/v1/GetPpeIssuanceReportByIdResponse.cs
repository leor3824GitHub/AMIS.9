namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed record PpeIssuanceLineItemResponse(
    string PropertyCode,
    string Description,
    double Quantity,
    string Unit,
    DateTime? DateAcquired,
    decimal AcquisitionCost,
    decimal? AccumulatedDepreciation,
    decimal? BookValue);

public sealed record GetPpeIssuanceReportByIdResponse(
    Guid Id,
    string ReportNumber,
    string RecipientName,
    string RecipientAddress,
    string IssuanceType,
    DateTime IssuanceDate,
    decimal TotalAcquisitionCost,
    int LineItemCount,
    int Status,
    string? Notes,
    IReadOnlyList<PpeIssuanceLineItemResponse> LineItems,
    DateTime CreatedOnUtc);

