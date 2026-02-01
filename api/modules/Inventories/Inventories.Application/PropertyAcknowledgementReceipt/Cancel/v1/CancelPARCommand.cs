using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Cancel.v1;

public sealed record CancelPARCommand(Guid Id) : IRequest<CancelPARResponse>;

public sealed record CancelPARResponse(
    Guid Id,
    string PARNumber,
    string Status,
    DateTime CancelledAt);
