using MediatR;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Update.v1;

public sealed record UpdateAssetRequisitionCommand(
    Guid Id,
    DateTime? ExpirationDate = null,
    string? Remarks = null) : IRequest<UpdateAssetRequisitionResponse>;

public sealed record UpdateAssetRequisitionResponse(
    Guid Id,
    DateTime? ExpirationDate,
    string? Remarks);

