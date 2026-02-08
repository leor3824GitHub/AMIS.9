namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed record GetPpeReceivingLineItemResponse(
    string PropertyCode,
    string RRNumber);

public sealed record GetPpeReceivingReportByIdResponse(
    Guid Id,
    string RRNumber,
    string ReceivedFrom,
    string Address,
    string Type,
    DateTime Date,
    decimal TotalAmount,
    string? Notes,
    int Status,
    IReadOnlyList<GetPpeReceivingLineItemResponse> Items);

