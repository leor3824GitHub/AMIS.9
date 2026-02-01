using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Update.v1;

public sealed record UpdatePpeReceivingReportCommand(
    Guid Id,
    IReadOnlyList<UpdatePpeReceivingLineItemRequest> LineItems,
    string SourceName,
    string SourceAddress,
    DateTime SourceReceiptDate,
    string ReceiptType,
    string? Notes = null) : IRequest<UpdatePpeReceivingReportResponse>;

public sealed record UpdatePpeReceivingLineItemRequest(
    string PropertyCode,
    string Description,
    DateTime DateAcquired,
    decimal Quantity,
    string Unit,
    decimal UnitCost,
    string Location,
    string? ClassCode = null,
    string? CategoryCode = null,
    string? ItemCode = null);

public sealed record UpdatePpeReceivingReportResponse(Guid Id);
