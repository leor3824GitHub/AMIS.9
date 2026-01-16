using MediatR;

namespace AMIS.Catalog.Application.Issuances.Features.Accept.v1;

public sealed record AcceptIssuanceCommand(
    Guid IssuanceId,
    Guid SignedByEmployeeId) : IRequest<AcceptIssuanceResponse>;
