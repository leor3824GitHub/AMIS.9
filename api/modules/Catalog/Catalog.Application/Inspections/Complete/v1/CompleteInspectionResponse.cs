using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.Complete.v1;

public sealed record CompleteInspectionResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message
);
