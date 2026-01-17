using MediatR;

namespace AMIS.Catalog.Application.Issuances.Features.Reject.v1;

public sealed record RejectIssuanceCommand(
    Guid IssuanceId,
    string RejectionReason) : IRequest<RejectIssuanceResponse>;
