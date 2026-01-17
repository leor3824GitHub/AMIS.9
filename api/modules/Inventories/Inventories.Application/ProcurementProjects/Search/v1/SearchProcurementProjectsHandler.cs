using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.ProcurementProjects.Search.v1;

public sealed class SearchProcurementProjectsHandler(
    [FromKeyedServices("inventories:procurementProjects")] IReadRepository<ProcurementProject> repository)
    : IRequestHandler<SearchProcurementProjectsCommand, PagedList<ProcurementProjectListItemResponse>>
{
    public async Task<PagedList<ProcurementProjectListItemResponse>> Handle(SearchProcurementProjectsCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchProcurementProjectsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        var responseItems = items
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ProcurementProjectListItemResponse(
                x.Id,
                x.PapCode,
                x.ProjectTitle,
                x.PmoEndUser,
                x.IsEpa,
                x.Mode,
                x.FundSource,
                x.Budget.TotalAmount))
            .ToList();

        return new PagedList<ProcurementProjectListItemResponse>(responseItems, request.PageNumber, request.PageSize, totalCount);
    }
}

