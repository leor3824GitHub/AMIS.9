using AMIS.Framework.Core.Caching;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;

public sealed class GetPpeCategoryCodeHandler(
    [FromKeyedServices("inventories:ppeCategoryCodes")] IReadRepository<PpeCategoryCode> repository,
    ICacheService cache)
    : IRequestHandler<GetPpeCategoryCodeRequest, PpeCategoryCodeResponse>
{
    public async Task<PpeCategoryCodeResponse> Handle(GetPpeCategoryCodeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"ppe-category-code:{request.Id}",
            async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (entity is null)
                    throw new PpeCategoryCodeNotFoundException(request.Id);

                return new PpeCategoryCodeResponse(
                    entity.Id,
                    entity.Code,
                    entity.AccountCode,
                    entity.Name,
                    entity.Description,
                    entity.SortOrder,
                    entity.IsActive,
                    entity.COAReference);
            },
            cancellationToken: cancellationToken);

        return item!;
    }
}
