using Ardalis.Specification;
using AMIS.WebApi.Inventories.Application.DepreciationSchedules.Search.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Search.v1;

public sealed class SearchDepreciationSchedulesSpecs : Specification<DepreciationSchedule, DepreciationScheduleDto>
{
    public SearchDepreciationSchedulesSpecs(SearchDepreciationSchedulesCommand request)
    {
        Query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created);

        Query
            .Select(s => new DepreciationScheduleDto(
                s.Id,
                s.PhysicalAssetId,
                s.Year,
                s.Month,
                s.MonthlyDepreciationAmount,
                s.AccumulatedDepreciationAmount,
                s.Status.ToString(),
                s.JournalEntryVoucherId));
    }
}

