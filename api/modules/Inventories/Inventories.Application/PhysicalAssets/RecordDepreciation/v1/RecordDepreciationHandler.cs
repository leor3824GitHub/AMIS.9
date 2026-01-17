using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.RecordDepreciation.v1;

public sealed class RecordDepreciationHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<RecordDepreciationCommand, RecordDepreciationResponse>
{
    public async Task<RecordDepreciationResponse> Handle(RecordDepreciationCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.AssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset with ID {request.AssetId} not found.");

        asset.RecordDepreciation(request.Amount, request.DepreciationDate);

        await repository.UpdateAsync(asset, cancellationToken);

        return new RecordDepreciationResponse(
            asset.Id,
            request.Amount,
            asset.AccumulatedDepreciation,
            asset.BookValue,
            request.DepreciationDate);
    }
}

