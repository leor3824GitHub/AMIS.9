using MediatR;

namespace AMIS.WebApi.Inventories.Application.Disposals.Request.v1;

/// <summary>
/// Command to request disposal of an asset
/// Initiated by Supply Officer or Asset Custodian
/// </summary>
public sealed record CreateDisposalRequestCommand(
    Guid PhysicalAssetId,
    string DisposalMethod,
    string AssetConditionAtDisposal,
    string? JustificationReason = null) : IRequest<CreateDisposalRequestResponse>;

public sealed record CreateDisposalRequestResponse(
    Guid DisposalId,
    string PropertyCode,
    string Status);
