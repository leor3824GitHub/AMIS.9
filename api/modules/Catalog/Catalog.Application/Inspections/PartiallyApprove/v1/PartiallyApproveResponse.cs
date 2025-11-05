using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.PartiallyApprove.v1;

public sealed record PartiallyApproveResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message,
    string PartialDetails
);
