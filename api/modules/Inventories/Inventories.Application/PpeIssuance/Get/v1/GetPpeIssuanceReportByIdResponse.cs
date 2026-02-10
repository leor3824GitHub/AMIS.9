namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed record PpeIssuanceLineItemResponse(
    string PropertyCode);

public sealed record GetPpeIssuanceReportByIdResponse(
    Guid Id,
    string IRNumber,
    string IssuedTo,
    string Address,
    string Type,
    DateTime Date,
    int LineItemCount,
    int Status,
    string? Notes,
    IReadOnlyList<PpeIssuanceLineItemResponse> LineItems);

