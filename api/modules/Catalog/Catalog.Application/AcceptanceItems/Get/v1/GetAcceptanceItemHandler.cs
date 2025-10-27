using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public sealed class GetAcceptanceItemHandler(
    [FromKeyedServices("catalog:acceptanceItems")] IReadRepository<AcceptanceItem> repository,
    ICacheService cache)
    : IRequestHandler<GetAcceptanceItemRequest, AcceptanceItemResponse>
{
    public async Task<AcceptanceItemResponse> Handle(GetAcceptanceItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var acceptance = await cache.GetOrSetAsync(
            $"acceptance:{request.Id}",
            async () =>
            {
                var spec = new GetAcceptanceItemSpecs(request.Id);
                var acceptanceItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (acceptanceItem == null) throw new AcceptanceItemNotFoundException(request.Id);
                return acceptanceItem;
            },
            cancellationToken: cancellationToken);

        return acceptance!;
    }
}
