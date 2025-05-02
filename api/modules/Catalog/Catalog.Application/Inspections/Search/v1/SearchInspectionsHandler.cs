using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Inspections.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Search.v1;

public sealed class SearchInspectionsHandler(
    [FromKeyedServices("catalog:inspections")] IReadRepository<Inspection> repository)
    : IRequestHandler<SearchInspectionsCommand, PagedList<InspectionResponse>>
{
    public async Task<PagedList<InspectionResponse>> Handle(SearchInspectionsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInspectionSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InspectionResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
