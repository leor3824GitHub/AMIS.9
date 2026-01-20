using MediatR;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed record ListPpeReceivingReportsQuery : IRequest<ListPpeReceivingReportsResponse>;
