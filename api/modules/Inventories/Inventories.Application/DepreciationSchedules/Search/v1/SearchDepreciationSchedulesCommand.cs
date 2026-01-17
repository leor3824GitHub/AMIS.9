using AMIS.Framework.Core.Paging;
using AMIS.WebApi.Inventories.Domain;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Search.v1;

public sealed record SearchDepreciationSchedulesCommand(
    int PageNumber = 1,
    int PageSize = 10) : IRequest<PagedList<DepreciationScheduleDto>>;

public sealed record DepreciationScheduleDto(
    Guid Id,
    Guid AssetId,
    int Year,
    int Month,
    decimal MonthlyDepreciationAmount,
    decimal AccumulatedDepreciationAmount,
    string Status,
    Guid? JournalEntryVoucherId);

