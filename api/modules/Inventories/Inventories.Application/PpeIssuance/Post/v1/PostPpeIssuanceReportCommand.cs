using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Post.v1;

public sealed record PostPpeIssuanceReportCommand(Guid Id) : IRequest<PostPpeIssuanceReportResponse>;

public sealed record PostPpeIssuanceReportResponse(Guid Id, string IRNumber, string Status);
