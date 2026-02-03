using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Search.v1;

public sealed class SearchPpeCategoryCodesHandler(
    [FromKeyedServices("inventories:ppeCategoryCodes")] IReadRepository<PpeCategoryCode> repository)
    : IRequestHandler<SearchPpeCategoryCodesCommand, PagedList<PpeCategoryCodeResponse>>
{
    public async Task<PagedList<PpeCategoryCodeResponse>> Handle(SearchPpeCategoryCodesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPpeCategoryCodeSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<PpeCategoryCodeResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
