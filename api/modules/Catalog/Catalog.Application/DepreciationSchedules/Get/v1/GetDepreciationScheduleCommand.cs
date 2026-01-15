using MediatR;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Get.v1;

public sealed record GetDepreciationScheduleCommand(Guid Id) : IRequest<GetDepreciationScheduleResponse>;

public sealed record GetDepreciationScheduleResponse(
    Guid Id,
    Guid AssetId,
    int Year,
    int Month,
    decimal MonthlyDepreciationAmount,
    decimal AccumulatedDepreciationAmount,
    string Status,
    Guid? JournalEntryVoucherId);
