using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Delete.v1;

public sealed record DeletePhysicalAssetCommand(Guid Id) : IRequest<DeletePhysicalAssetResponse>;

public sealed record DeletePhysicalAssetResponse(Guid Id);

public sealed class DeletePhysicalAssetHandler(
    [FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    : IRequestHandler<DeletePhysicalAssetCommand, DeletePhysicalAssetResponse>
{
    public async Task<DeletePhysicalAssetResponse> Handle(
        DeletePhysicalAssetCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var physicalAsset = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Physical asset {request.Id} not found");

        await repository.DeleteAsync(physicalAsset, cancellationToken);

        return new DeletePhysicalAssetResponse(physicalAsset.Id);
    }
}

