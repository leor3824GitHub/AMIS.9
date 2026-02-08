using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Post.v1;

public sealed record PostPpeReceivingReportCommand(Guid Id) : IRequest<PostPpeReceivingReportResponse>;

public sealed record PostPpeReceivingReportResponse(Guid Id, string RRNumber, string Status);
