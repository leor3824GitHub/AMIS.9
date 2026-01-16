using MediatR;

namespace AMIS.Catalog.Application.Issuances.Features.Return.v1;

public sealed record ReturnIssuanceCommand(Guid IssuanceId) : IRequest<ReturnIssuanceResponse>;
