using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.RequireReInspection.v1;

public sealed record RequireReInspectionCommand(
    Guid InspectionId,
    string Reason
) : IRequest<RequireReInspectionResponse>;
