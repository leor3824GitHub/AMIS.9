using MediatR;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.GenerateQRCode.v1;

public sealed record GenerateQRCodeCommand(
    Guid AssetId,
    string QRCodeData) : IRequest<GenerateQRCodeResponse>;

public sealed record GenerateQRCodeResponse(
    Guid AssetId,
    DateTime GeneratedDate);

