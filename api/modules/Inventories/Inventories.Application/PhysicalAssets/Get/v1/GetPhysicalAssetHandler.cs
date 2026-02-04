using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Get.v1;

public sealed class GetPhysicalAssetHandler(
    [FromKeyedServices("inventories:physicalassets")] IReadRepository<PhysicalAsset> repository)
    : IRequestHandler<GetPhysicalAssetCommand, PhysicalAssetResponse>
{
    public async Task<PhysicalAssetResponse> Handle(
        GetPhysicalAssetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var physicalAsset = await repository.FirstOrDefaultAsync(new GetPhysicalAssetSpec(request.Id), cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        return physicalAsset;
    }
}

