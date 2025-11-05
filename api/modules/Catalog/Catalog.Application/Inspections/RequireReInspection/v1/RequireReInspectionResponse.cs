using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.RequireReInspection.v1;

public sealed record RequireReInspectionResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message,
    string Reason
);
