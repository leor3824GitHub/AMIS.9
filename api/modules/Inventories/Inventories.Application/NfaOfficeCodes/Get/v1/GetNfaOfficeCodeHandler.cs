using AMIS.Framework.Core.Caching;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;

public sealed class GetNfaOfficeCodeHandler(
    [FromKeyedServices("inventories:nfaOfficeCodes")] IReadRepository<NfaOfficeCode> repository,
    ICacheService cache)
    : IRequestHandler<GetNfaOfficeCodeRequest, NfaOfficeCodeResponse>
{
    public async Task<NfaOfficeCodeResponse> Handle(GetNfaOfficeCodeRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var item = await cache.GetOrSetAsync(
            $"nfa-office-code:{request.Id}",
            async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
                if (entity is null)
                    throw new NfaOfficeCodeNotFoundException(request.Id);

                return new NfaOfficeCodeResponse(
                    entity.Id,
                    entity.Code,
                    entity.OfficeName,
                    entity.Description,
                    entity.ParentOfficeCode,
                    entity.SortOrder,
                    entity.IsActive);
            },
            cancellationToken: cancellationToken);

        return item!;
    }
}
