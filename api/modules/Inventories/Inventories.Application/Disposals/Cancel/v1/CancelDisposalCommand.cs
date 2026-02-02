using MediatR;

namespace AMIS.WebApi.Inventories.Application.Disposals.Cancel.v1;

/// <summary>
/// Command to cancel a disposal request
/// Can be cancelled from Pending or Approved state
/// </summary>
public sealed record CancelDisposalCommand(
    Guid DisposalId,
    string? CancellationReason = null) : IRequest<CancelDisposalResponse>;

public sealed record CancelDisposalResponse(
    Guid DisposalId,
    string PropertyCode,
    string Status,
    DateTime CancelledOn);
