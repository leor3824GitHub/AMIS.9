using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;

public sealed class GetAcceptanceItemHandler(
    [FromKeyedServices("catalog:inspectionItems")] IReadRepository<AcceptanceItem> repository,
    ICacheService cache)
    : IRequestHandler<GetAcceptanceItemRequest, AcceptanceItemResponse>
{
    public async Task<AcceptanceItemResponse> Handle(GetAcceptanceItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await cache.GetOrSetAsync(
            $"inspection:{request.Id}",
            async () =>
            {
                var spec = new GetAcceptanceItemSpecs(request.Id);
                var inspectionItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inspectionItem == null) throw new AcceptanceItemNotFoundException(request.Id);
                return inspectionItem;
            },
            cancellationToken: cancellationToken);

        return inspection!;
    }
}
