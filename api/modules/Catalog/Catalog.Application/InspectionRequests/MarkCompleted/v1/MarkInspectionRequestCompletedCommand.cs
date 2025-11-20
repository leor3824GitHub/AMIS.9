using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;

public sealed record MarkInspectionRequestCompletedCommand(Guid Id) : IRequest<MarkInspectionRequestCompletedResponse>;
public sealed record MarkInspectionRequestCompletedResponse(Guid Id);
