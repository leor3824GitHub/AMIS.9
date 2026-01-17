using MediatR;

namespace AMIS.Inventories.Application.Issuances.Features.Cancel.v1;

public sealed record CancelIssuanceCommand(Guid IssuanceId) : IRequest<CancelIssuanceResponse>;
