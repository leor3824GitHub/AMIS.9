using MediatR;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Accept.v1;

public sealed record AcceptAssetRequisitionCommand(
    Guid Id,
    string SignatureData,
    string IpAddress,
    string UserAgent,
    string? DeviceFingerprint = null) : IRequest<AcceptAssetRequisitionResponse>;

public sealed record AcceptAssetRequisitionResponse(Guid Id);
