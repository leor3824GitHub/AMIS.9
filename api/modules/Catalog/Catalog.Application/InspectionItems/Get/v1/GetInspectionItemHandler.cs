using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;

public sealed class GetInspectionItemHandler(
    [FromKeyedServices("catalog:inspectionItems")] IReadRepository<InspectionItem> repository,
    ICacheService cache)
    : IRequestHandler<GetInspectionItemRequest, InspectionItemResponse>
{
    public async Task<InspectionItemResponse> Handle(GetInspectionItemRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await cache.GetOrSetAsync(
            $"inspection:{request.Id}",
            async () =>
            {
                var spec = new GetInspectionItemSpecs(request.Id);
                var inspectionItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inspectionItem == null) throw new InspectionItemNotFoundException(request.Id);
                return inspectionItem;
            },
            cancellationToken: cancellationToken);

        return inspection!;
    }
}
