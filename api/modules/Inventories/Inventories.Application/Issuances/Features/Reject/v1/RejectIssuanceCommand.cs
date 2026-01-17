using MediatR;

namespace AMIS.Inventories.Application.Issuances.Features.Reject.v1;

public sealed record RejectIssuanceCommand(
    Guid IssuanceId,
    string RejectionReason) : IRequest<RejectIssuanceResponse>;
