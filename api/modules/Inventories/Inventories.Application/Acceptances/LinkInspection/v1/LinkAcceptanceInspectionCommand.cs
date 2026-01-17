using MediatR;

namespace AMIS.WebApi.Inventories.Application.Acceptances.LinkInspection.v1;

public sealed record LinkAcceptanceInspectionCommand(Guid AcceptanceId, Guid InspectionId) : IRequest<LinkAcceptanceInspectionResponse>;
public sealed record LinkAcceptanceInspectionResponse(Guid AcceptanceId);

