using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Search.v1;

public sealed class SearchDepreciationSchedulesHandler(
    [FromKeyedServices("catalog:depreciationschedules")] IReadRepository<DepreciationSchedule> repository)
    : IRequestHandler<SearchDepreciationSchedulesCommand, PagedList<DepreciationScheduleDto>>
{
    public async Task<PagedList<DepreciationScheduleDto>> Handle(SearchDepreciationSchedulesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchDepreciationSchedulesSpecs(request);

        var schedules = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<DepreciationScheduleDto>(schedules, request.PageNumber, request.PageSize, totalCount);
    }
}
