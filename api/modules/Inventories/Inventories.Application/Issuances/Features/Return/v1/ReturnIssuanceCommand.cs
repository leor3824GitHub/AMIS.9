using MediatR;

namespace AMIS.Inventories.Application.Issuances.Features.Return.v1;

public sealed record ReturnIssuanceCommand(Guid IssuanceId) : IRequest<ReturnIssuanceResponse>;
