namespace AMIS.Inventories.Application.Issuances.Features.Reject.v1;

public sealed record RejectIssuanceResponse(
    Guid IssuanceId,
    string RejectionReason);
