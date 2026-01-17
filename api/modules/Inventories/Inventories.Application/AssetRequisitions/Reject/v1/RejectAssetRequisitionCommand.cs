using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Reject.v1;

public sealed record RejectAssetRequisitionCommand(
    Guid Id,
    string Reason) : IRequest<RejectAssetRequisitionResponse>;

public sealed record RejectAssetRequisitionResponse(
    Guid Id,
    string Status,
    string RejectionReason);

