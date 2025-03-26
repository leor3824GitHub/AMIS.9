using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Get.v1;
public sealed class GetIssuanceItemHandler(
    [FromKeyedServices("catalog:issuanceItems")] IReadRepository<IssuanceItem> repository,
    ICacheService cache)
    : IRequestHandler<GetIssuanceItemRequest, IssuanceItemResponse>
{
    public async Task<IssuanceItemResponse> Handle(GetIssuanceItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"issuanceItem:{request.Id}",
            async () =>
            {
                var spec = new GetIssuanceItemSpecs(request.Id);
                var issuanceItemItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (issuanceItemItem == null) throw new IssuanceItemNotFoundException(request.Id);
                return issuanceItemItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
