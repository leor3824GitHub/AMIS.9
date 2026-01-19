using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Cancel.v1;

public sealed record CancelPpeReceivingReportCommand(Guid Id) : IRequest<CancelPpeReceivingReportResponse>;

public sealed record CancelPpeReceivingReportResponse(
    Guid Id,
    string Status,
    string Message);
