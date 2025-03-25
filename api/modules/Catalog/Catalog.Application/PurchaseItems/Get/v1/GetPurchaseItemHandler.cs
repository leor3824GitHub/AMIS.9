using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Get.v1;
public sealed class GetPurchaseItemHandler(
    [FromKeyedServices("catalog:purchaseItems")] IReadRepository<PurchaseItem> repository,
    ICacheService cache)
    : IRequestHandler<GetPurchaseItemRequest, PurchaseItemResponse>
{
    public async Task<PurchaseItemResponse> Handle(GetPurchaseItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"purchaseItem:{request.Id}",
            async () =>
            {
                var spec = new GetPurchaseItemSpecs(request.Id);
                var purchaseItemItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (purchaseItemItem == null) throw new PurchaseItemNotFoundException(request.Id);
                return purchaseItemItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
