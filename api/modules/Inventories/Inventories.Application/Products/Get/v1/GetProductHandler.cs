using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.Products.Get.v1;
public sealed class GetProductHandler(
    [FromKeyedServices("inventories:products")] IReadRepository<Product> repository,
    ICacheService cache)
    : IRequestHandler<GetProductRequest, ProductResponse>
{
    public async Task<ProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"product:{request.Id}",
            async () =>
            {
                var spec = new GetProductSpecs(request.Id);
                var productItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (productItem == null) throw new ProductNotFoundException(request.Id);
                return productItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}

