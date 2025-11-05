using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.MarkAccepted.v1;

public sealed record MarkAcceptedResponse(
    Guid InspectionRequestId,
    InspectionRequestStatus Status,
    string Message
);
