using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.PartiallyApprove.v1;

public sealed record PartiallyApproveCommand(
    Guid InspectionId,
    string PartialDetails
) : IRequest<PartiallyApproveResponse>;
