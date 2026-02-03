using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Search.v1;

public sealed class SearchNfaOfficeCodesHandler(
    [FromKeyedServices("inventories:nfaOfficeCodes")] IReadRepository<NfaOfficeCode> repository)
    : IRequestHandler<SearchNfaOfficeCodesCommand, PagedList<NfaOfficeCodeResponse>>
{
    public async Task<PagedList<NfaOfficeCodeResponse>> Handle(SearchNfaOfficeCodesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchNfaOfficeCodeSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<NfaOfficeCodeResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
