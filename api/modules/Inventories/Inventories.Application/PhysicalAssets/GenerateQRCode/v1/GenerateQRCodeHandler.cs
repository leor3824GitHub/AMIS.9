using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.GenerateQRCode.v1;

public sealed class GenerateQRCodeHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<GenerateQRCodeCommand, GenerateQRCodeResponse>
{
    public async Task<GenerateQRCodeResponse> Handle(GenerateQRCodeCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.AssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset with ID {request.AssetId} not found.");

        asset.GenerateQRCode(request.QRCodeData, request.PropertyNumber);

        await repository.UpdateAsync(asset, cancellationToken);

        return new GenerateQRCodeResponse(
            asset.Id,
            asset.PropertyNumber!,
            asset.QRGeneratedDate!.Value);
    }
}

