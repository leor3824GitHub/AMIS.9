using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Schedule.v1;

public sealed record ScheduleInspectionCommand(
    Guid InspectionId,
    DateTime ScheduledDate) : IRequest<ScheduleInspectionResponse>;

public sealed record ScheduleInspectionResponse(
    Guid InspectionId,
    string Status,
    DateTime? ScheduledDate,
    string Message);
