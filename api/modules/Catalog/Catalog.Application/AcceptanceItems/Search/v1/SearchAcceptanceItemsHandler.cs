using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Search.v1;
public sealed class SearchAcceptanceItemsHandler(
    [FromKeyedServices("catalog:acceptanceItems")] IReadRepository<AcceptanceItem> repository)
    : IRequestHandler<SearchAcceptanceItemsCommand, PagedList<AcceptanceItemResponse>>
{
    public async Task<PagedList<AcceptanceItemResponse>> Handle(SearchAcceptanceItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchAcceptanceItemSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<AcceptanceItemResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

