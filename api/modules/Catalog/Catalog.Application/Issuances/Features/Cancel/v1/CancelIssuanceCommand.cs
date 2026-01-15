using MediatR;

namespace AMIS.Catalog.Application.Issuances.Features.Cancel.v1;

public sealed record CancelIssuanceCommand(Guid IssuanceId) : IRequest<CancelIssuanceResponse>;
