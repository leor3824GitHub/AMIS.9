using MediatR;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Create.v1;

public sealed record CreateDepreciationScheduleCommand(
    Guid AssetId,
    int Year,
    int Month,
    decimal MonthlyDepreciationAmount) : IRequest<CreateDepreciationScheduleResponse>;

public sealed record CreateDepreciationScheduleResponse(Guid Id);

