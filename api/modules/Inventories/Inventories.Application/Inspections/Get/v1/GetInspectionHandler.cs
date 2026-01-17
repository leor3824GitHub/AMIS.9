using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inspections.Get.v1;

public sealed class GetInspectionHandler(
    [FromKeyedServices("inventories:inspections")] IReadRepository<Inspection> repository,
    ICacheService cache)
    : IRequestHandler<GetInspectionRequest, InspectionResponse>
{
    public async Task<InspectionResponse> Handle(GetInspectionRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await cache.GetOrSetAsync(
            $"inspection:{request.Id}",
            async () =>
            {
                var spec = new GetInspectionSpecs(request.Id);
                var inspectionItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inspectionItem == null) throw new InspectionNotFoundException(request.Id);
                return inspectionItem;
            },
            cancellationToken: cancellationToken);

        return inspection!;
    }
}

