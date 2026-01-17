namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed record GetPpeReceivingReportByIdResponse(
    Guid Id,
    string ReportNumber,
    string Location,
    string SourceName,
    string ReceiptType,
    decimal TotalAmount,
    string? Notes);

