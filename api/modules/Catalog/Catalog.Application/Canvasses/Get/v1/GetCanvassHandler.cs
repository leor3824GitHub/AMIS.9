using Microsoft.Extensions.DependencyInjection;
using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Get.v1;

public sealed class GetCanvassHandler(
    [FromKeyedServices("catalog:canvasses")] IReadRepository<Canvass> repository,
    ICacheService cache)
    : IRequestHandler<GetCanvassRequest, CanvassResponse>
{
    public async Task<CanvassResponse> Handle(GetCanvassRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"canvass:{request.Id}",
            async () =>
            {
                var spec = new GetCanvassSpecs(request.Id);
                var canvass = await repository.FirstOrDefaultAsync(spec, cancellationToken)
                    ?? throw new NotFoundException($"Canvass with ID {request.Id} not found.");

                return new CanvassResponse(
                    canvass.Id,
                    canvass.PurchaseRequestId,
                    canvass.SupplierId,
                    canvass.ItemDescription,
                    canvass.Quantity,
                    canvass.Unit,
                    canvass.QuotedPrice,
                    canvass.Remarks,
                    canvass.ResponseDate,
                    canvass.IsSelected);
            },
            cancellationToken: cancellationToken);

        return item!;
    }
}
