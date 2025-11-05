using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromQuarantine.v1;

public sealed record ReleaseInspectionFromQuarantineCommand(Guid InspectionId) : IRequest<ReleaseInspectionFromQuarantineResponse>;
