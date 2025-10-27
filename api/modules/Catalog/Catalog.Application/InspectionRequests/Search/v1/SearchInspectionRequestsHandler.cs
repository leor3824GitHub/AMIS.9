using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.InspectionRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Search.v1;

public sealed class SearchInspectionRequestsHandler(
    [FromKeyedServices("catalog:inspectionRequests")] IReadRepository<InspectionRequest> repository)
    : IRequestHandler<SearchInspectionRequestsCommand, PagedList<InspectionRequestResponse>>
{
    public async Task<PagedList<InspectionRequestResponse>> Handle(SearchInspectionRequestsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchInspectionRequestSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<InspectionRequestResponse>(items, request.PageNumber, request.PageSize, totalCount);
    }
}
