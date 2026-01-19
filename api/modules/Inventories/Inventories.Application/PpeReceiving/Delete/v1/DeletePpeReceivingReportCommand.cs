using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Delete.v1;

public sealed record DeletePpeReceivingReportCommand(Guid Id) : IRequest<DeletePpeReceivingReportResponse>;

public sealed record DeletePpeReceivingReportResponse(
    Guid Id,
    string Message);
