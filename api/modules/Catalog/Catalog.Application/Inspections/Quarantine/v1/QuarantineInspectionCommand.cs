using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.Quarantine.v1;

public sealed record QuarantineInspectionCommand(
    Guid InspectionId,
    string Reason) : IRequest<QuarantineInspectionResponse>;

public sealed record QuarantineInspectionResponse(
    Guid InspectionId,
    string Status,
    string Message);
