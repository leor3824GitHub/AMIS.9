namespace AMIS.Catalog.Application.Issuances.Features.Reject.v1;

public sealed record RejectIssuanceResponse(
    Guid IssuanceId,
    string RejectionReason);
