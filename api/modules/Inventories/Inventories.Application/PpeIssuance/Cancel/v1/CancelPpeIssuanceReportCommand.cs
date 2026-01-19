using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Cancel.v1;

public sealed record CancelPpeIssuanceReportCommand(Guid Id) : IRequest<CancelPpeIssuanceReportResponse>;

public sealed record CancelPpeIssuanceReportResponse(
    Guid Id,
    string Status,
    string Message);
