using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromQuarantine.v1;

public sealed record ReleaseInspectionFromQuarantineResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message
);
