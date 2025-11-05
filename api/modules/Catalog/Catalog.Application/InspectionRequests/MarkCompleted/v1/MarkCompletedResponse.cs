using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkCompleted.v1;

public sealed record MarkCompletedResponse(
    Guid InspectionRequestId,
    InspectionRequestStatus Status,
    string Message
);
