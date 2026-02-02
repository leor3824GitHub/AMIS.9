using MediatR;

namespace AMIS.WebApi.Inventories.Application.Disposals.Approve.v1;

/// <summary>
/// Command to approve a pending disposal request
/// Authorized personnel (Manager/Supervisor) can approve disposals
/// </summary>
public sealed record ApproveDisposalCommand(
    Guid DisposalId,
    string? ApprovalNotes = null) : IRequest<ApproveDisposalResponse>;

public sealed record ApproveDisposalResponse(
    Guid DisposalId,
    string PropertyCode,
    string Status,
    DateTime ApprovedOn);
