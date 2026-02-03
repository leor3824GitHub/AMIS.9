using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;

public sealed record CreatePpeReceivingReportCommand(
    string ReportNumber,
    IReadOnlyList<CreatePpeReceivingLineItemRequest> LineItems,
    string SourceName,
    string SourceAddress,
    DateTime SourceReceiptDate,
    string ReceiptType,
    string? Notes = null) : IRequest<CreatePpeReceivingReportResponse>;

public sealed record CreatePpeReceivingLineItemRequest(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string? PpeType = null,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null);

public sealed record CreatePpeReceivingReportResponse(Guid? Id);

