using MediatR;

namespace AMIS.WebApi.Catalog.Application.PhysicalAssets.GenerateQRCode.v1;

public sealed record GenerateQRCodeCommand(
    Guid AssetId,
    string QRCodeData,
    string? PropertyNumber = null) : IRequest<GenerateQRCodeResponse>;

public sealed record GenerateQRCodeResponse(
    Guid AssetId,
    string PropertyNumber,
    DateTime GeneratedDate);
