using MediatR;

namespace AMIS.Inventories.Application.Issuances.Features.Accept.v1;

public sealed record AcceptIssuanceCommand(
    Guid IssuanceId,
    Guid SignedByEmployeeId) : IRequest<AcceptIssuanceResponse>;
