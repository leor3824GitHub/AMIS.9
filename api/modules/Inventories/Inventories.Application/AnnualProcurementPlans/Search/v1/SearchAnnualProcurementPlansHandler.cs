using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Search.v1;

public sealed class SearchAnnualProcurementPlansHandler(
    ILogger<SearchAnnualProcurementPlansHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IReadRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<SearchAnnualProcurementPlansCommand, PagedList<AnnualProcurementPlanListItemResponse>>
{
    public async Task<PagedList<AnnualProcurementPlanListItemResponse>> Handle(SearchAnnualProcurementPlansCommand request, CancellationToken cancellationToken)
    {
        var spec = new SearchAnnualProcurementPlansSpec(request);
        var filter = new PaginationFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            OrderBy = request.OrderBy,
            Keyword = request.Keyword,
        };

        var result = await repository.PaginatedListAsync(spec, filter, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Searched annual procurement plans - page {PageNumber}", request.PageNumber);
        return result;
    }
}

