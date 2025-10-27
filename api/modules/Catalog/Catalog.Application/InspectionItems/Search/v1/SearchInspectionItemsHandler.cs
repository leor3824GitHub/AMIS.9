using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.InspectionItems.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace AMIS.WebApi.Catalog.Application.InspectionItems.Search.v1;
public sealed class SearchInspectionItemsHandler(
    [FromKeyedServices("catalog:inspectionItems")] IReadRepository<InspectionItem> repository)
    : IRequestHandler<SearchInspectionItemsCommand, PagedList<InspectionItemResponse>>
{
    public async Task<PagedList<InspectionItemResponse>> Handle(SearchInspectionItemsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInspectionItemSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InspectionItemResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}

