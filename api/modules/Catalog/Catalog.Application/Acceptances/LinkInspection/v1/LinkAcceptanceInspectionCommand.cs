using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.LinkInspection.v1;

public sealed record LinkAcceptanceInspectionCommand(Guid AcceptanceId, Guid InspectionId) : IRequest<LinkAcceptanceInspectionResponse>;
public sealed record LinkAcceptanceInspectionResponse(Guid AcceptanceId);
