using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Inspections.ConditionallyApprove.v1;

public sealed record ConditionallyApproveResponse(
    Guid InspectionId,
    InspectionStatus Status,
    string Message,
    string Conditions
);
