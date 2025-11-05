using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.ReleaseFromHold.v1;

public sealed record ReleaseInspectionFromHoldCommand(Guid InspectionId) : IRequest<ReleaseInspectionFromHoldResponse>;
