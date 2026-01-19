using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Delete.v1;

public sealed record DeletePpeIssuanceReportCommand(Guid Id) : IRequest<DeletePpeIssuanceReportResponse>;

public sealed record DeletePpeIssuanceReportResponse(
    Guid Id,
    string Message);
