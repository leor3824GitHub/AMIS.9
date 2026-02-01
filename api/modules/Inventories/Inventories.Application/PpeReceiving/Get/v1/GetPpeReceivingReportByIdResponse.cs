namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed record GetPpeReceivingLineItemResponse(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string? ClassCode,
    string? CategoryCode,
    string? ItemCode);

public sealed record GetPpeReceivingReportByIdResponse(
    Guid Id,
    string ReportNumber,
    string SourceName,
    string SourceAddress,
    string ReceiptType,
    DateTime SourceReceiptDate,
    decimal TotalAmount,
    string? Notes,
    int Status,
    IReadOnlyList<GetPpeReceivingLineItemResponse> LineItems);

