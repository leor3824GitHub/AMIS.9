using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;

public sealed class SearchPurchaseRequestsHandler(
    ILogger<SearchPurchaseRequestsHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IReadRepository<PurchaseRequest> repository)
    : IRequestHandler<SearchPurchaseRequestsCommand, AMIS.Framework.Core.Paging.PagedList<PurchaseRequestResponse>>
{
    public async Task<AMIS.Framework.Core.Paging.PagedList<PurchaseRequestResponse>> Handle(SearchPurchaseRequestsCommand request, CancellationToken cancellationToken)
    {
        var specs = new SearchPurchaseRequestSpecs(request);
        var filter = new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize, OrderBy = request.OrderBy, Keyword = request.Keyword };
        var results = await repository.PaginatedListAsync(specs, filter, cancellationToken);
        logger.LogInformation("Searched PurchaseRequests - page {PageNumber}", request.PageNumber);
        return results;
    }
}
