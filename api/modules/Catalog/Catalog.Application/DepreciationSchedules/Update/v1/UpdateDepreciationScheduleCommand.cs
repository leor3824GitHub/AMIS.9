using MediatR;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Update.v1;

public sealed record UpdateDepreciationScheduleCommand(
    Guid Id,
    string? Remarks = null) : IRequest<UpdateDepreciationScheduleResponse>;

public sealed record UpdateDepreciationScheduleResponse(
    Guid Id,
    string? Remarks);
