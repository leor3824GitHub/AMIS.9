using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromHold.v1;

public sealed record ReleaseInspectionFromHoldResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message
);
