using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
public sealed class GetIssuanceHandler(
    [FromKeyedServices("catalog:issuances")] IReadRepository<Issuance> repository,
    ICacheService cache)
    : IRequestHandler<GetIssuanceRequest, IssuanceResponse>
{
    public async Task<IssuanceResponse> Handle(GetIssuanceRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"issuance:{request.Id}",
            async () =>
            {
                var spec = new GetIssuanceSpecs(request.Id);
                var issuanceItem = await repository.FirstOrDefaultAsync(spec, cancellationToken);
                if (issuanceItem == null) throw new IssuanceNotFoundException(request.Id);
                return issuanceItem;
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
