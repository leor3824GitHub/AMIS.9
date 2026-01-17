using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.AssignCustodian.v1;

public sealed class AssignCustodianHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<AssignCustodianCommand, AssignCustodianResponse>
{
    public async Task<AssignCustodianResponse> Handle(AssignCustodianCommand request, CancellationToken cancellationToken)
    {
        var asset = await repository.GetByIdAsync(request.AssetId, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset with ID {request.AssetId} not found.");

        asset.AssignToCustodian(request.CustodianId);

        await repository.UpdateAsync(asset, cancellationToken);

        return new AssignCustodianResponse(
            asset.Id,
            asset.CurrentCustodianId!.Value,
            DateTime.UtcNow);
    }
}

