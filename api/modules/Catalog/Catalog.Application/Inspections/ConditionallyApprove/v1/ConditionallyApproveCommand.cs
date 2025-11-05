using MediatR;

namespace AMIS.WebApi.Catalog.Application.Inspections.ConditionallyApprove.v1;

public sealed record ConditionallyApproveCommand(
    Guid InspectionId,
    string Conditions
) : IRequest<ConditionallyApproveResponse>;
