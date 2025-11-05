using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.PutOnHold.v1;

public sealed record PutInspectionOnHoldCommand(
    Guid InspectionId,
    string Reason
) : IRequest<PutInspectionOnHoldResponse>;
