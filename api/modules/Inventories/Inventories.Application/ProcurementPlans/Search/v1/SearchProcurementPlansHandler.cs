using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Search.v1;

public sealed class SearchProcurementPlansHandler(
    ILogger<SearchProcurementPlansHandler> logger,
    [FromKeyedServices("inventories:procurementPlans")] IReadRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<SearchProcurementPlansCommand, PagedList<ProcurementPlanListItemResponse>>
{
    public async Task<PagedList<ProcurementPlanListItemResponse>> Handle(SearchProcurementPlansCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchProcurementPlansSpec(request);
        var filter = new PaginationFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            OrderBy = request.OrderBy,
            Keyword = request.Keyword,
        };

        var result = await repository.PaginatedListAsync(spec, filter, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Searched procurement plans - page {PageNumber}", request.PageNumber);
        return result;
    }
}

