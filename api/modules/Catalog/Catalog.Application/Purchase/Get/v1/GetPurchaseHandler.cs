using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Get.v1;
public sealed class GetPurchaseHandler(
    [FromKeyedServices("catalog:purchases")] IReadRepository<Purchase> repository,
    ICacheService cache)
    : IRequestHandler<GetPurchaseRequest, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(GetPurchaseRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"purchase:{request.Id}",
            async () =>
            {
                var spec = new GetPurchaseSpecs(request.Id);
                var purchaseItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (purchaseItem == null) throw new PurchaseNotFoundException(request.Id);
                return purchaseItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
