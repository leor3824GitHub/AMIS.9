using MediatR;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Cancel.v1;

public sealed record CancelAssetRequisitionCommand(Guid Id) : IRequest<CancelAssetRequisitionResponse>;

public sealed record CancelAssetRequisitionResponse(
    Guid Id,
    string Status);
