using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.PutOnHold.v1;

public sealed record PutInspectionOnHoldResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message,
    string Reason
);
