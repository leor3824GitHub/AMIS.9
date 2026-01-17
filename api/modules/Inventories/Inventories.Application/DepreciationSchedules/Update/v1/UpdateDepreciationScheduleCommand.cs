using MediatR;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Update.v1;

public sealed record UpdateDepreciationScheduleCommand(
    Guid Id,
    string? Remarks = null) : IRequest<UpdateDepreciationScheduleResponse>;

public sealed record UpdateDepreciationScheduleResponse(
    Guid Id,
    string? Remarks);

