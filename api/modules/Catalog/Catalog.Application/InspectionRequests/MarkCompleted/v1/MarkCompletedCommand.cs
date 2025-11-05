using MediatR;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;

public sealed record MarkCompletedCommand(Guid InspectionRequestId) : IRequest<MarkCompletedResponse>;
