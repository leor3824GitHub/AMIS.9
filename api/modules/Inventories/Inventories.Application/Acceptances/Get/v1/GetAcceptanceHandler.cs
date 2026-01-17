using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using AMIS.Framework.Core.Caching;
using MediatR;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Get.v1;

public sealed class GetAcceptanceHandler(
    [FromKeyedServices("inventories:acceptances")] IReadRepository<Acceptance> repository,
    ICacheService cache)
    : IRequestHandler<GetAcceptanceRequest, AcceptanceResponse>
{
    public async Task<AcceptanceResponse> Handle(GetAcceptanceRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await cache.GetOrSetAsync(
            $"inspection:{request.Id}",
            async () =>
            {
                var spec = new GetAcceptanceSpecs(request.Id);
                var inspectionItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (inspectionItem == null) throw new AcceptanceNotFoundException(request.Id);
                return inspectionItem;
            },
            cancellationToken: cancellationToken);

        return inspection!;
    }
}

