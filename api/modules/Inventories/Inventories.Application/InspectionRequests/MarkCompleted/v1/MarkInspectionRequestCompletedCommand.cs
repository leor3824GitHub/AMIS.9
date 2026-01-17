using MediatR;

namespace AMIS.WebApi.Inventories.Application.InspectionRequests.MarkCompleted.v1;

public sealed record MarkInspectionRequestCompletedCommand(Guid Id) : IRequest<MarkInspectionRequestCompletedResponse>;
public sealed record MarkInspectionRequestCompletedResponse(Guid Id);

