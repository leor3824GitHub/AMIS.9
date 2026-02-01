using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Return.v1;

public sealed record ReturnPARCommand(
    Guid Id,
    DateTime ReturnDate,
    Guid ReceivedByEmployeeId,
    string ReceivedByEmployeeName,
    string? ReturnRemarks = null) : IRequest<ReturnPARResponse>;

public sealed record ReturnPARResponse(
    Guid Id,
    string PARNumber,
    string Status,
    DateTime ReturnedAt);
